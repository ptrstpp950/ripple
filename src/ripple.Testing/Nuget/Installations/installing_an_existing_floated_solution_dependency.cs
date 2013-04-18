﻿using FubuTestingSupport;
using NUnit.Framework;
using ripple.Model;
using ripple.Nuget;

namespace ripple.Testing.Nuget.Installations
{
    [TestFixture]
    public class installing_an_existing_floated_solution_dependency : NugetPlanContext
    {
        private SolutionGraphScenario theScenario;
        private Solution theSolution;
        private NugetPlan thePlan;
        private NugetPlanBuilder theBuilder;

        [SetUp]
        public void SetUp()
        {
            FeedScenario.Create(scenario => scenario.For(Feed.Fubu).Add("fubu", "1.0.0.1"));

            theScenario = SolutionGraphScenario.Create(scenario =>
            {
                scenario.Solution("Test", test =>
                {
                    test.LocalDependency("fubu", "1.0.0.1");
                });
            });

            theSolution = theScenario.Find("Test");

            theBuilder = new NugetPlanBuilder();

            var request = new NugetPlanRequest
            {
                Solution = theSolution,
                Dependency = new Dependency("fubu"),
                Operation = OperationType.Install,
                Mode = UpdateMode.Float
            };

            thePlan = theBuilder.PlanFor(request);
        }

        [TearDown]
        public void TearDown()
        {
            theScenario.Cleanup();
            FeedRegistry.Reset();
        }
        
        [Test]
        public void no_installation()
        {
            thePlan.ShouldHaveCount(0);
        }
    }
}