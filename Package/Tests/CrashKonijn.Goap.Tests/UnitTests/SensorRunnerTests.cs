﻿using CrashKonijn.Goap.Classes.Runners;
using CrashKonijn.Goap.Core.Interfaces;
using CrashKonijn.Goap.UnitTests.Classes;
using NSubstitute;
using NUnit.Framework;

namespace CrashKonijn.Goap.UnitTests
{
    public class SensorRunnerTests
    {
        [Test]
        public void SenseGlobal_GlobalSensor_CallsSense()
        {
            // Arrange
            var sensor = Substitute.For<IGlobalWorldSensor>();
            
            var runner = new SensorRunner(new IWorldSensor[] { sensor }, new ITargetSensor[] { }, new IMultiSensor[] { }, Substitute.For<IGlobalWorldData>());
            
            // Act
            runner.SenseGlobal();
            
            // Assert
            sensor.Received().Sense(Arg.Any<IWorldData>());
        }
        
        [Test]
        public void SenseGlobal_LocalSensor_DoesntCallsSense()
        {
            // Arrange
            var sensor = Substitute.For<ILocalWorldSensor>();
            
            var runner = new SensorRunner(new IWorldSensor[] { sensor }, new ITargetSensor[] { }, new IMultiSensor[] { }, Substitute.For<IGlobalWorldData>());
            
            // Act
            runner.SenseGlobal();
            
            // Assert
            sensor.DidNotReceive().Sense(Arg.Any<IWorldData>(), Arg.Any<IMonoAgent>(), Arg.Any<IComponentReference>());
        }
        
        [Test]
        public void SenseGlobal_MultiSensor_CallsSense()
        {
            // Arrange
            var sensor = Substitute.For<IMultiSensor>();
            
            var runner = new SensorRunner(new IWorldSensor[] {  }, new ITargetSensor[] { }, new IMultiSensor[] { sensor }, Substitute.For<IGlobalWorldData>());
            
            // Act
            runner.SenseGlobal();
            
            // Assert
            sensor.Received().Sense(Arg.Any<IWorldData>());
        }
        
        [Test]
        public void SenseLocal_GlobalSensor_DoesntCallsSense()
        {
            // Arrange
            var sensor = Substitute.For<IGlobalWorldSensor>();
            
            var runner = new SensorRunner(new IWorldSensor[] { sensor }, new ITargetSensor[] { }, new IMultiSensor[] { }, Substitute.For<IGlobalWorldData>());
            
            var agent = Substitute.For<IMonoAgent>();
            agent.WorldData.Returns(new LocalWorldData());
            
            // Act
            runner.SenseLocal(agent);
            
            // Assert
            sensor.DidNotReceive().Sense(Arg.Any<IWorldData>());
        }
        
        [Test]
        public void SenseLocal_LocalSensor_CallsSense()
        {
            // Arrange
            var sensor = Substitute.For<ILocalWorldSensor>();
            
            var runner = new SensorRunner(new IWorldSensor[] { sensor }, new ITargetSensor[] { }, new IMultiSensor[] { }, Substitute.For<IGlobalWorldData>());
            
            var agent = Substitute.For<IMonoAgent>();
            agent.WorldData.Returns(new LocalWorldData());
            
            // Act
            runner.SenseLocal(agent);
            
            // Assert
            sensor.Received().Sense(Arg.Any<IWorldData>(), agent, Arg.Any<IComponentReference>());
        }
        
        [Test]
        public void SenseLocal_MultiSensor_CallsSense()
        {
            // Arrange
            var sensor = Substitute.For<IMultiSensor>();
            
            var runner = new SensorRunner(new IWorldSensor[] {  }, new ITargetSensor[] { }, new IMultiSensor[] { sensor }, Substitute.For<IGlobalWorldData>());
            
            var agent = Substitute.For<IMonoAgent>();
            agent.WorldData.Returns(new LocalWorldData());
            
            // Act
            runner.SenseLocal(agent);
            
            // Assert
            sensor.Received().Sense(Arg.Any<IWorldData>(), agent, Arg.Any<IComponentReference>());
        }

        [Test]
        public void SenseLocal_WithAgent_OnlyRunsMatchingKey()
        {
            // Arrange
            var key = new TestKey1();
            
            var matchedSensor = Substitute.For<ILocalWorldSensor>();
            matchedSensor.Key.Returns(key);
            
            var unMatchedSensor = Substitute.For<ILocalWorldSensor>();
            unMatchedSensor.Key.Returns(new TestKey2());
            
            var condition = Substitute.For<ICondition>();
            condition.WorldKey.Returns(key);
            
            var action = Substitute.For<IAction>();
            action.Conditions.Returns(new []{ condition });
            
            var runner = new SensorRunner(new IWorldSensor[] { matchedSensor, unMatchedSensor }, new ITargetSensor[] { }, new IMultiSensor[] { }, Substitute.For<IGlobalWorldData>());
            
            // Act
            runner.SenseLocal(Substitute.For<IMonoAgent>(), action);
            
            // Assert
            matchedSensor.Received().Sense(Arg.Any<IWorldData>(), Arg.Any<IMonoAgent>(), Arg.Any<IComponentReference>());
            unMatchedSensor.DidNotReceive().Sense(Arg.Any<IWorldData>(), Arg.Any<IMonoAgent>(), Arg.Any<IComponentReference>());
        }
    }
}