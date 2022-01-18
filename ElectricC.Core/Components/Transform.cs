using ElectricC.Core.MathUtils;
using ElectricC.ECS;
using OpenTK;
using System;

namespace ElectricC.Core.Components
{
    public class Transform: Component
    {
        public Transform Parent { get; set; }
        public Matrix4 ParentMatrix
        {
            get
            {
                return Parent != null ? Parent.Transformation : Matrix4.Identity;
            }
        }

        public Vector3 Position;
        public Vector3 Rotation;
        public Vector3 Scale;

        public Matrix4 Transformation { get => GetTransformation(); }
        public Vector3 TransformedPosition => Position + (Parent == null ? new Vector3(0, 0, 0) : Parent.TransformedPosition);
        public Vector3 Forward { get => GetForward(); }
        public Vector3 Right { get => GetRight(); }

        public Transform()
        {
            Position = new Vector3(0, 0, 0);
            Rotation = new Vector3(0, 0, 0);
            Scale = new Vector3(1, 1, 1);
        }

        private Matrix4 GetTransformation()
        {
            Matrix4 translation = Matrix4.CreateTranslation(Position);
            Matrix4 rotation = GetRotationMatrix();
            Matrix4 scale = Matrix4.CreateScale(Scale);

            return ((scale * rotation)) * translation * ParentMatrix;
        }

        public Matrix4 GetRotationMatrix()
        {
            Matrix4 rotationX = Matrix4.CreateRotationX(Rotation.X.
                ToRadians());
            Matrix4 rotationY = Matrix4.CreateRotationY(Rotation.Y.ToRadians());
            Matrix4 rotationZ = Matrix4.CreateRotationZ(Rotation.Z.ToRadians());

            return rotationZ * rotationY * rotationX;
        }

        public Matrix4 GetTranslationMatrix()
        {
            return Matrix4.CreateTranslation(Position);
        }

        public Matrix4 GetScaleMatrix()
        {
            return Matrix4.CreateScale(Scale);
        }

        private Vector3 GetForward()
        {
            return new Vector3(
                (float)Math.Cos(MathHelper.DegreesToRadians(Rotation.X)) * (float)Math.Cos(MathHelper.DegreesToRadians(Rotation.Y)),
                (float)Math.Sin(MathHelper.DegreesToRadians(Rotation.X)),
                (float)Math.Cos(MathHelper.DegreesToRadians(Rotation.X)) * (float)Math.Sin(MathHelper.DegreesToRadians(Rotation.Y))
            ).Normalized();
        }

        private Vector3 GetRight()
        {
            return Vector3.Cross(Forward, Vector3.UnitY);
        }

        public void RotateX(float angle)
        {
            Rotation.X += angle;
        }

        public void RotateY(float angle)
        {
            Rotation.Y += angle;
        }

        public void RotateZ(float angle)
        {
            Rotation.Z += angle;
        }

        public void Rotate(Vector3 rotation)
        {
            Rotation += rotation;
        }

        public void TranslateX(float translation)
        {
            Position.X += translation;
        }

        public void TranslateY(float translation)
        {
            Position.Y += translation;
        }

        public void TranslateZ(float translation)
        {
            Position.Z += translation;
        }

        public void Translate(Vector3 translation)
        {
            Position += translation;
        }

        public void ScaleX(float scalar)
        {
            Scale.X += scalar;
        }

        public void ScaleY(float scalar)
        {
            Scale.Y += scalar;
        }

        public void ScaleZ(float scalar)
        {
            Scale.Z += scalar;
        }

        public void Resize(Vector3 scalar)
        {
            Scale += scalar;
        }
    }
}
