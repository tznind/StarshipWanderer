using System;
using System.IO;
using NUnit.Framework;
using Wanderer;
using Wanderer.Actions;
using Wanderer.Compilation;
using Wanderer.Factories;
using Wanderer.Factories.Blueprints;

namespace  Tests
{
    public class TestAction : Wanderer.Actions.Action
    {
        public TestAction(IHasStats owner):base(owner)
        {

        }
        
        public override void Push(IWorld world, IUserinterface ui, ActionStack stack, Wanderer.Actors.IActor actor)
        {
            if(ui.GetChoice("Destroy the world",null,out bool chosen,new []{true,false}))
                if(chosen)
                {
                    stack.Push(new Frame(actor,this,0));
                }
        }

        public override void Pop(IWorld world, IUserinterface ui, ActionStack stack, Frame frame)
        {
            world.Player.Kill(ui,stack.Round,"fate");
        }
    }
    public class TestCustomAction
    {

        string yaml = @"
Name: Unleash
Type: Tests.TestAction";

        [Ignore("Not currently working :(")]
        [Test]
        public void TestCustomAction_Create()
        {
            var blue = Compiler.Instance.Deserializer.Deserialize<ActionBlueprint>(yaml);

            var dir = Path.Combine( TestContext.CurrentContext.TestDirectory,"EmptyFolder");
            Directory.CreateDirectory(dir);

            var wf = new WorldFactory(){ResourcesDirectory = dir};
            var world = wf.Create();


            var f = new ActionFactory();
            var action = f.Create(world,world.Player,blue);
            
            Assert.IsInstanceOf(typeof(TestAction),action);
        }
    }    
}

