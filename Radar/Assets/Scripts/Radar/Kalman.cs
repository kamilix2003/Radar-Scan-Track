using System;
using UnityEngine;

namespace kalman
{
public class RadarEKF
{
    private Vector3 position;
    private Vector3 velocity;

    private float dt;
    private Matrix P; // 6x6 covariance matrix
    private Matrix Q; // process noise
    private Matrix R; // measurement noise (2x2)

    public RadarEKF(float deltaTime)
    {
        dt = deltaTime;

        position = Vector3.zero;
        velocity = Vector3.zero;

        P = Matrix.Identity(6) * 1000f;
        Q = Matrix.Identity(6) * 0.1f;
        R = Matrix.Identity(2) * 10f;
    }

    // Prediction step
    public void Predict()
    {
        position += velocity * dt;

        // Jacobian of the motion model F
        Matrix F = Matrix.Identity(6);
        F[0, 3] = dt;
        F[1, 4] = dt;
        F[2, 5] = dt;

        P = F * P * F.Transpose() + Q;
    }

    // Update step with radar measurement (range, radial velocity)
    public void Update(float measuredRange, float measuredRadialVelocity)
    {
        Vector3 pos = position;
        Vector3 vel = velocity;

        float range = pos.magnitude;
        if (range < 1e-4f) return; // Avoid division by zero

        float radialVelocity = Vector3.Dot(pos, vel) / range;

        Vector2 z = new Vector2(measuredRange, measuredRadialVelocity);
        Vector2 zPred = new Vector2(range, radialVelocity);

        Vector2 y = z - zPred;

        // Measurement Jacobian H (2x6)
        Matrix H = new Matrix(2, 6);

        // Partial derivatives for range
        H[0, 0] = pos.x / range;
        H[0, 1] = pos.y / range;
        H[0, 2] = pos.z / range;

        // Partial derivatives for radial velocity
        float dot = Vector3.Dot(pos, vel);
        float range2 = range * range;

        H[1, 0] = (vel.x * range - dot * pos.x / range) / range2;
        H[1, 1] = (vel.y * range - dot * pos.y / range) / range2;
        H[1, 2] = (vel.z * range - dot * pos.z / range) / range2;

        H[1, 3] = pos.x / range;
        H[1, 4] = pos.y / range;
        H[1, 5] = pos.z / range;

        // Kalman gain
        Matrix S = H * P * H.Transpose() + R;
        Matrix K = P * H.Transpose() * S.Inverse();

        // Update state
        Vector dx = K * new Vector(new float[] {y.x, y.y });
        position += new Vector3(dx[0], dx[1], dx[2]);
        velocity += new Vector3(dx[3], dx[4], dx[5]);

        // Update covariance
        Matrix I = Matrix.Identity(6);
        P = (I - K * H) * P;
    }

    public Vector3 GetPosition() => position;
    public Vector3 GetVelocity() => velocity;

    }
public class Matrix
    {
        private readonly float[,] data;

        public int Rows { get; private set; }
        public int Cols { get; private set; }

        public float this[int i, int j]
        {
            get => data[i, j];
            set => data[i, j] = value;
        }

        public Matrix(int rows, int cols)
        {
            Rows = rows;
            Cols = cols;
            data = new float[rows, cols];
        }

        public static Matrix Identity(int size)
        {
            var result = new Matrix(size, size);
            for (int i = 0; i < size; i++)
                result[i, i] = 1f;
            return result;
        }

        public Matrix Transpose()
        {
            var result = new Matrix(Cols, Rows);
            for (int i = 0; i < Rows; i++)
                for (int j = 0; j < Cols; j++)
                    result[j, i] = data[i, j];
            return result;
        }

        public Matrix Inverse()
        {
            if (Rows != Cols)
                throw new InvalidOperationException("Only square matrices can be inverted.");

            int n = Rows;
            var result = Identity(n);
            var copy = new Matrix(n, n);
            Array.Copy(data, copy.data, data.Length);

            for (int i = 0; i < n; i++)
            {
                float diag = copy[i, i];
                if (Math.Abs(diag) < 1e-6)
                    throw new InvalidOperationException("Matrix is singular or near-singular.");

                for (int j = 0; j < n; j++)
                {
                    copy[i, j] /= diag;
                    result[i, j] /= diag;
                }

                for (int k = 0; k < n; k++)
                {
                    if (k == i) continue;
                    float factor = copy[k, i];
                    for (int j = 0; j < n; j++)
                    {
                        copy[k, j] -= factor * copy[i, j];
                        result[k, j] -= factor * result[i, j];
                    }
                }
            }

            return result;
        }

        // Matrix * Matrix
        public static Matrix operator *(Matrix A, Matrix B)
        {
            if (A.Cols != B.Rows)
                throw new InvalidOperationException("Matrix dimensions do not match.");

            var result = new Matrix(A.Rows, B.Cols);
            for (int i = 0; i < A.Rows; i++)
                for (int j = 0; j < B.Cols; j++)
                    for (int k = 0; k < A.Cols; k++)
                        result[i, j] += A[i, k] * B[k, j];
            return result;
        }

        // Matrix * Vector
        public static Vector operator *(Matrix A, Vector v)
        {
            if (A.Cols != v.Length)
                throw new InvalidOperationException("Matrix and vector size mismatch.");

            float[] result = new float[A.Rows];
            for (int i = 0; i < A.Rows; i++)
                for (int j = 0; j < A.Cols; j++)
                    result[i] += A[i, j] * v[j];
            return new Vector(result);
        }

        // Matrix + Matrix
        public static Matrix operator +(Matrix A, Matrix B)
        {
            if (A.Rows != B.Rows || A.Cols != B.Cols)
                throw new InvalidOperationException("Matrix sizes do not match.");

            var result = new Matrix(A.Rows, A.Cols);
            for (int i = 0; i < A.Rows; i++)
                for (int j = 0; j < A.Cols; j++)
                    result[i, j] = A[i, j] + B[i, j];
            return result;
        }

        // Matrix - Matrix
        public static Matrix operator -(Matrix A, Matrix B)
        {
            if (A.Rows != B.Rows || A.Cols != B.Cols)
                throw new InvalidOperationException("Matrix sizes do not match.");

            var result = new Matrix(A.Rows, A.Cols);
            for (int i = 0; i < A.Rows; i++)
                for (int j = 0; j < A.Cols; j++)
                    result[i, j] = A[i, j] - B[i, j];
            return result;
        }

        // Matrix * scalar
        public static Matrix operator *(Matrix A, float scalar)
        {
            var result = new Matrix(A.Rows, A.Cols);
            for (int i = 0; i < A.Rows; i++)
                for (int j = 0; j < A.Cols; j++)
                    result[i, j] = A[i, j] * scalar;
            return result;
        }

        // Scalar * Matrix
        public static Matrix operator *(float scalar, Matrix A) => A * scalar;
    }
public class Vector
    {
        private readonly float[] data;

        public int Length => data.Length;

        public float this[int i]
        {
            get => data[i];
            set => data[i] = value;
        }

        public Vector(float[] values)
        {
            data = new float[values.Length];
            Array.Copy(values, data, values.Length);
        }

        public static Vector operator -(Vector a, Vector b)
        {
            if (a.Length != b.Length)
                throw new InvalidOperationException("Vector sizes do not match.");
            float[] result = new float[a.Length];
            for (int i = 0; i < a.Length; i++)
                result[i] = a[i] - b[i];
            return new Vector(result);
        }

        public static Vector operator +(Vector a, Vector b)
        {
            if (a.Length != b.Length)
                throw new InvalidOperationException("Vector sizes do not match.");
            float[] result = new float[a.Length];
            for (int i = 0; i < a.Length; i++)
                result[i] = a[i] + b[i];
            return new Vector(result);
        }

        public static Vector operator *(Vector v, float scalar)
        {
            float[] result = new float[v.Length];
            for (int i = 0; i < v.Length; i++)
                result[i] = v[i] * scalar;
            return new Vector(result);
        }

        public static Vector operator *(float scalar, Vector v) => v * scalar;

        public float[] ToArray() => (float[])data.Clone();
    }
}

