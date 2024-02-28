using Engine;
using Engine.Graphics;
using System.Numerics;
using TestApp;
using Vortice.Direct3D11;

AppHost.InitializeServices();
Scene scene = new();

var solidMaterial = new BasicMaterial()
{
    Albedo = Texture.White1x1,
};

var cubeModel = Assets.LoadMesh("Models/cube.obj");

var diceMaterial = new BasicMaterial
{
    Albedo = Assets.LoadTexture("Textures/dice.png"),
};
var diceModel = Assets.LoadMesh("Models/dice.fbx");
scene.AddActor([new MeshRenderer(diceModel, diceMaterial)]).Transform.Position = new(-1.5f, 0, 1.5f);
scene.AddActor([new MeshRenderer(diceModel, diceMaterial)]).Transform.Position = new(1.5f, 0, -1.5f);

var light = scene.AddActor([new MeshRenderer(cubeModel, solidMaterial)]);
light.Transform.Position = new(0, 2, 0);
light.Transform.Rotation = Quaternion.CreateFromYawPitchRoll(0, MathF.PI / 4f, MathF.PI / 4f);
light.Transform.Scale = new(.5f);

var floor = scene.AddActor([new MeshRenderer(Mesh.Plane, solidMaterial)]);
floor.Transform.Position = new(0, -1.1f, 0);
floor.Transform.Scale = new(20);

var cameraActor = scene.AddActor([new Camera(), new FlyingCamera()]);
cameraActor.Transform.Position = Vector3.UnitZ;
cameraActor.Transform.Rotate(Vector3.UnitY, MathF.PI);

Scenes.Start(scene);
AppHost.Run();