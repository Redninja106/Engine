
using Engine.Graphics;

namespace Engine;

public class Scene
{
    private readonly Queue<Actor> actorsToAdd = new();
    private readonly Queue<Actor> actorsToRemove = new();
    private readonly List<Actor> actors = new();
    private readonly List<Camera> cameras = new();

    public IEnumerable<Camera> ActiveCameras => cameras;
    public IEnumerable<Actor> Actors => actors;

    public void Update(float deltaTime)
    {
        foreach (var actor in actors)
        {
            actor.Update(deltaTime);
        }

        while (actorsToAdd.Count > 0)
        {
            actors.Add(actorsToAdd.Dequeue());
        }

        while (actorsToRemove.Count > 0)
        {
            actors.Remove(actorsToRemove.Dequeue());
        }
    }

    public void Draw(RenderContext context)
    {
        foreach (var actor in actors)
        {
            actor.Draw(context);
        }
    }

    public Actor AddActor(ReadOnlySpan<Component> components)
    {
        foreach (var component in components)
        {
            if (component is Camera camera)
                cameras.Add(camera);
        }

        Actor actor = new(components);
        actorsToAdd.Enqueue(actor);
        return actor;
    }

    public IEnumerable<TComponent> FindAll<TComponent>()
        where TComponent : Component
    {
        foreach (var actor in actors)
        {
            foreach (var component in actor.Components)
            {
                if (component is TComponent c)
                    yield return c;
            }
            
        }
    }
}