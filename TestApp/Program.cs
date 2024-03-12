using Engine;
using Engine.Graphics;
using System.Numerics;
using TestApp;
using Vortice.Direct3D11;

AppHost.InitializeServices();
Scene scene = new();

var solidMaterial = new BasicMaterial();
solidMaterial.Albedo.Set(Texture.White1x1);

// var cubeModel = Assets.LoadMesh("Models/cube.obj");

var diceMaterial = new BasicMaterial();
diceMaterial.Albedo.Set(Assets.LoadTexture("Textures/dice.png"));

var diceModel = Assets.LoadMesh("Models/dice.fbx");
scene.AddActor("dice 1", [new MeshRenderer(diceModel, diceMaterial)]).Transform.Position = new(-1.5f, 0, 1.5f);
scene.AddActor("dice 2", [new MeshRenderer(diceModel, diceMaterial)]).Transform.Position = new(1.5f, 0, -1.5f);

var light = scene.AddActor("light", [new PointLight()]);
var light2 = scene.AddActor([new PointLight()]);
scene.AddActor([new PointLight()]);
light.Transform.Position = new(0, 2, 0);
light.Transform.Rotation = Quaternion.CreateFromYawPitchRoll(0, MathF.PI / 4f, MathF.PI / 4f);
light.Transform.Scale = new(.5f);

var floor = scene.AddActor("floor", [new Ground(), new MeshRenderer(Mesh.Plane, solidMaterial)]);
floor.Transform.Position = new(0, -1.1f, 0);

var cameraActor = scene.AddActor("camera", [new Camera(new BasicRenderDriver()), new FlyingCamera()]);
cameraActor.Transform.Position = Vector3.UnitZ;
cameraActor.Transform.Rotate(Vector3.UnitY, MathF.PI);

Scenes.Start(scene);
AppHost.Run();

