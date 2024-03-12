using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Vortice.Mathematics;

namespace Engine;
public class Transform : Component
{
    // (parent) -> position -> rotation -> scale -> (children)

    private Transform? parent;
    private Vector3 position;
    private Vector3 scale;
    private Quaternion rotation;

    public ref Vector3 Position => ref position;
    public ref Vector3 Scale => ref scale;
    public ref Quaternion Rotation => ref rotation;

    public Transform? Parent => parent;

    public Vector3 WorldPosition
    {
        get
        {
            if (this.parent is null)
                return this.position;

            return this.parent.LocalToWorld(this.position);
        }
        set
        {
            if (this.parent is null)
            {
                this.position = value;
                return;
            }

            this.position = this.parent.WorldToLocal(value);
        }
    }

    public Quaternion WorldRotation
    {
        get
        {
            if (this.parent is null)
            {
                return this.rotation;
            }
            else
            {
                return Quaternion.Concatenate(this.parent.WorldRotation, this.rotation);
            }
        }
        set
        {
            if (this.parent is null)
            {
                this.rotation = value;
            }
            else
            {
                this.rotation = Quaternion.Concatenate(value, Quaternion.Inverse(this.parent.WorldRotation));
            }
        }
    }

    public Vector3 WorldScale
    {
        get
        {
            if (this.parent is null)
            {
                return this.scale;
            }
            else
            {
                return this.parent.WorldScale * this.scale;
            }
        }
        set
        {
            if (this.parent is null)
            {
                this.scale = value;
            }
            else
            {
                this.scale = value / this.parent.WorldScale;
            }
        }
    }

    public Vector3 Right
    {
        get => Vector3.Transform(Vector3.UnitX, Rotation);
    }

    public Vector3 Left
    {
        get => Vector3.Transform(-Vector3.UnitX, Rotation);
    }

    public Vector3 Up
    {
        get => Vector3.Transform(Vector3.UnitY, Rotation);
    }

    public Vector3 Down
    {
        get => Vector3.Transform(-Vector3.UnitY, Rotation);
    }

    public Vector3 Forward
    {
        get => Vector3.Transform(Vector3.UnitZ, Rotation);
    }

    public Vector3 Backward
    {
        get => Vector3.Transform(-Vector3.UnitZ, Rotation);
    }

    public Transform(Transform? parent = null) : this(Vector3.Zero, Quaternion.Identity, parent)
    {
    }

    public Transform(Vector3 position, Quaternion rotation, Transform? parent = null) : this(position, rotation, Vector3.One, parent)
    {
    }

    public Transform(Vector3 position, Quaternion rotation, Vector3 scale, Transform? parent = null)
    {
        this.position = position;
        this.rotation = rotation;
        this.scale = scale;
        this.parent = parent;
    }

    public override void Update(float deltaTime)
    {
    }

    public void Translate(float x, float y, float z, bool local = true)
    {
        Translate(new(x, y, z), local);
    }

    public void Translate(Vector3 translation, bool local = true)
    {
        if (local)
        {
            Position += translation;
        }
        else
        {
            WorldPosition += translation;
        }
    }

    public void Rotate(Vector3 axis, float angle)
    {
        Rotation = Quaternion.Concatenate(Rotation, Quaternion.CreateFromAxisAngle(axis, angle));
    }

    public void Rotate(float yaw, float pitch, float roll)
    {
        Rotation = Quaternion.Concatenate(Rotation, Quaternion.CreateFromYawPitchRoll(yaw, pitch, roll));
    }

    public void ScaleBy(Vector3 scale)
    {
        this.scale *= scale;
    }

    public Vector3 WorldToLocal(Vector3 point)
    {
        return Vector3.Transform(point, CreateWorldToLocalMatrix());
    }

    public Vector3 LocalToWorld(Vector3 point)
    {
        return Vector3.Transform(point, CreateLocalToWorldMatrix());
    }

    public Vector3 ParentToLocal(Vector3 point)
    {
        return Vector3.Transform(point, CreateParentToLocalMatrix());
    }

    public Vector3 LocalToParent(Vector3 point)
    {
        return Vector3.Transform(point, CreateLocalToParentMatrix());
    }

    public Matrix4x4 CreateWorldToLocalMatrix()
    {
        if (parent is null)
            return CreateParentToLocalMatrix();

        return parent.CreateWorldToLocalMatrix() * this.CreateParentToLocalMatrix();
    }

    public Matrix4x4 CreateLocalToWorldMatrix()
    {
        if (parent is null)
            return CreateLocalToParentMatrix();

        return this.CreateLocalToParentMatrix() * parent.CreateLocalToWorldMatrix();
    }

    public Matrix4x4 CreateParentToLocalMatrix()
    {
        var matrix = Matrix4x4.CreateScale(Vector3.One / this.Scale);
        matrix = Matrix4x4.CreateFromQuaternion(Quaternion.Inverse(this.Rotation)) * matrix;
        matrix = Matrix4x4.CreateTranslation(-this.Position) * matrix;
        return matrix;
    }

    public Matrix4x4 CreateLocalToParentMatrix()
    {
        var matrix = Matrix4x4.CreateTranslation(this.Position);
        matrix = Matrix4x4.CreateFromQuaternion(this.Rotation) * matrix;
        matrix = Matrix4x4.CreateScale(this.Scale) * matrix;
        return matrix;
    }

    public void Match(Transform other)
    {
        this.Position = other.Position;
        this.Rotation = other.Rotation;
        this.Scale = other.Scale;
    }

    public void LerpTowards(Transform other, float t)
    {
        this.Position = Vector3.Lerp(this.Position, other.Position, t);
        this.Rotation = Quaternion.Slerp(this.Rotation, other.Rotation, t);
        this.Scale = Vector3.Lerp(this.Scale, other.Scale, t);
    }

    public void Reparent(Transform? newParent)
    {
        this.parent = newParent;
    }

    public void LookAt(Vector3 point)
    {
        Vector3 delta = point - position;
        if (delta.LengthSquared() == 0)
            return;

        Vector3 lookDirection = Vector3.Normalize(delta);

        float pitch = -MathF.Asin(lookDirection.Y);
        float yaw = MathF.Atan2(lookDirection.X, lookDirection.Z);

        this.rotation = Quaternion.CreateFromYawPitchRoll(yaw, pitch, 0);
    }

    public void StepTowards(Vector3 point, float distance)
    {
        Vector3 diff = point - position;
        float lenSq = point.LengthSquared();

        if (lenSq < distance * distance)
        {
            position = point;
            return;
        }

        diff *= MathF.ReciprocalSqrtEstimate(lenSq);
        position += diff * distance;
    }

    public void TurnTowards(Vector2 point, float radians, bool local = true)
    {
        throw new NotImplementedException();
        /*if (local)
        {
            this.rotation = Angle.Step(this.rotation, Angle.FromVector(point - this.position), radians);
        }
        else
        {
            this.WorldRotation = Angle.Step(this.WorldRotation, Angle.FromVector(point - this.WorldPosition), radians);
        }*/
    }

    public override void Layout()
    {
        base.Layout();

        ImGui.Text("Position");
        ImGui.SameLine();
        ImGui.DragFloat3("##position slider", ref this.Position);

        ImGui.Text("Rotation");
        ImGui.SameLine();
        Vector3 euler = Rotation.ToEuler();
        if (ImGui.DragFloat3("##rotation slider", ref euler)) 
        {
            Rotation = Quaternion.CreateFromYawPitchRoll(euler.Y, euler.X, euler.Z);
        }

        ImGui.Text("Scale");
        ImGui.SameLine();
        ImGui.DragFloat3("##scale slider", ref this.Scale);
    }
}