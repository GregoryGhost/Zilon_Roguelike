﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:2.4.0.0
//      SpecFlow Generator Version:2.4.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace Zilon.Core.Spec.TestCases
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.4.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [TechTalk.SpecRun.FeatureAttribute("Fight_Dual_SuccessRoll", Description="\tЧтобы ввести разнообразные сбособы экипировки и развития\r\n\tКак разработчику\r\n\tМн" +
        "е нужно, чтобы персонаж мог экипироваться и атаковать двумя оружиями.", SourceFile="TestCases\\Fight_Dual_SuccessRoll.feature", SourceLine=0)]
    public partial class Fight_Dual_SuccessRollFeature
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "Fight_Dual_SuccessRoll.feature"
#line hidden
        
        [TechTalk.SpecRun.FeatureInitialize()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Fight_Dual_SuccessRoll", "\tЧтобы ввести разнообразные сбособы экипировки и развития\r\n\tКак разработчику\r\n\tМн" +
                    "е нужно, чтобы персонаж мог экипироваться и атаковать двумя оружиями.", ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [TechTalk.SpecRun.FeatureCleanup()]
        public virtual void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        public virtual void TestInitialize()
        {
        }
        
        [TechTalk.SpecRun.ScenarioCleanup()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioInitialize(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioInitialize(scenarioInfo);
        }
        
        public virtual void ScenarioStart()
        {
            testRunner.OnScenarioStart();
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        public virtual void УспешныйУдарДвумяОружиями_(string personSid, string monsterSid, string monsterId, string monsterNodeX, string monsterNodeY, string propSid1, string slotIndex1, string propSid2, string slotIndex2, string expectedMonsterHp, string[] exampleTags)
        {
            string[] @__tags = new string[] {
                    "fight",
                    "dev0"};
            if ((exampleTags != null))
            {
                @__tags = System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Concat(@__tags, exampleTags));
            }
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Успешный удар двумя оружиями.", null, @__tags);
#line 7
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 8
 testRunner.Given("Есть карта размером 10", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 9
 testRunner.And(string.Format("Есть актёр игрока класса {0} в ячейке (0, 0)", personSid), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 10
 testRunner.And(string.Format("Есть монстр класса {0} Id:{1} в ячейке ({2}, {3})", monsterSid, monsterId, monsterNodeX, monsterNodeY), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 11
 testRunner.And(string.Format("Актёр игрока экипирован предметом {0} в слот Index: {1}", propSid1, slotIndex1), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 12
 testRunner.And(string.Format("Актёр игрока экипирован предметом {0} в слот Index: {1}", propSid2, slotIndex2), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 13
 testRunner.When(string.Format("Актёр игрока атакует монстра Id:{0}", monsterId), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 16
 testRunner.Then(string.Format("Монстр Id:{0} имеет Hp {1}", monsterId, expectedMonsterHp), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [TechTalk.SpecRun.ScenarioAttribute("Успешный удар двумя оружиями., human-person", new string[] {
                "fight",
                "dev0"}, SourceLine=19)]
        public virtual void УспешныйУдарДвумяОружиями__Human_Person()
        {
#line 7
this.УспешныйУдарДвумяОружиями_("human-person", "skorolon", "1000", "0", "1", "short-sword", "2", "short-sword", "3", "4", ((string[])(null)));
#line hidden
        }
        
        [TechTalk.SpecRun.TestRunCleanup()]
        public virtual void TestRunCleanup()
        {
            TechTalk.SpecFlow.TestRunnerManager.GetTestRunner().OnTestRunEnd();
        }
    }
}
#pragma warning restore
#endregion
