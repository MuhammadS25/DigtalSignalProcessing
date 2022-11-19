using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DSPAlgorithms.DataStructures;
using DSPAlgorithms.Algorithms;

namespace DSPComponentsUnitTest
{
    /// <summary>
    /// Summary description for ShiftingAndFolding
    /// </summary>
    [TestClass]
    public class ShiftingAndFolding
    {
        Signal inputSignal, expectedOutputSignal, actualOutputSignal;

        Shifter s = new Shifter();
        Folder f = new Folder();

        [TestInitialize]
        public void ShiftingAndFoldingInitializer()
        {
            inputSignal = UnitTestUtitlities.LoadSignal("TestingSignals/Input_ShiftFold.ds");
        }

        [TestCleanup]
        public void ShiftingAndFoldingValidation()
        {
            Assert.IsTrue(UnitTestUtitlities.SignalsSamplesIndicesAreEqual(expectedOutputSignal, actualOutputSignal));
        }

        [TestMethod]
        public void ShiftLeftTestMethod1()
        {
            s.InputSignal = inputSignal;
            s.ShiftingValue = 500;
            expectedOutputSignal = UnitTestUtitlities.LoadSignal("TestingSignals/Output_Shift_Plus500.ds");
            s.Run();
            actualOutputSignal = s.OutputShiftedSignal;
 
        }

        [TestMethod]
        public void ShiftingRightTestMethod2()
        {
            s.InputSignal = inputSignal;
            s.ShiftingValue = -500;
            expectedOutputSignal = UnitTestUtitlities.LoadSignal("TestingSignals/Output_Shift_Minus500.ds");
            s.Run();
            actualOutputSignal = s.OutputShiftedSignal;
         
        }
       
        [TestMethod]
        public void FoldingTestMethod3()
        {
            f.InputSignal = inputSignal;
            expectedOutputSignal = UnitTestUtitlities.LoadSignal("TestingSignals/Output_Fold.ds");
            f.Run();
            actualOutputSignal = f.OutputFoldedSignal;
        }

        [TestMethod]
        public void FoldingAndShiftRightTestMethod4()
        {
            s.ShiftingValue = 500;
            expectedOutputSignal = UnitTestUtitlities.LoadSignal("TestingSignals/Output_Fold_Plus500.ds");
            f.InputSignal = inputSignal;
            f.Run();
            s.InputSignal = f.OutputFoldedSignal;
            s.Run();
            actualOutputSignal = s.OutputShiftedSignal;
        }

        [TestMethod]
        public void FoldingAndShiftLeftTestMethod5()
        {
            s.ShiftingValue = -500;
            expectedOutputSignal = UnitTestUtitlities.LoadSignal("TestingSignals/Output_Fold_Minus500.ds");
            f.InputSignal = inputSignal;
            f.Run();
            s.InputSignal = f.OutputFoldedSignal;
            s.Run();
            actualOutputSignal = s.OutputShiftedSignal;
        }

        [TestMethod]
        public void ShiftRightThenFoldingTestMethod6()
        {
            s.ShiftingValue = -500;
            //expectedOutputSignal = UnitTestUtitlities.LoadSignal("TestingSignals/Output_Fold_Plus500.ds");
            expectedOutputSignal = UnitTestUtitlities.LoadSignal("TestingSignals/Output_Fold_Minus500.ds");
            s.InputSignal = inputSignal;
            s.Run();
            f.InputSignal = s.OutputShiftedSignal;
            f.Run();
            actualOutputSignal = f.OutputFoldedSignal;
            
        }

        [TestMethod]
        public void ShiftLeftThenFoldingTestMethod7()
        {
            s.ShiftingValue = 500;
            //expectedOutputSignal = UnitTestUtitlities.LoadSignal("TestingSignals/Output_Fold_Minus500.ds");
            expectedOutputSignal = UnitTestUtitlities.LoadSignal("TestingSignals/Output_Fold_Plus500.ds");
            s.InputSignal = inputSignal;
            s.Run();
            f.InputSignal = s.OutputShiftedSignal;
            f.Run();
            actualOutputSignal = f.OutputFoldedSignal;
           
        }
        
        [TestMethod]
        public void FoldingShiftRightFoldingShiftLeftTestMethod8()
        {
            // {1, 2, 3, 8}
            //  -1 0, 1, 2
            f.InputSignal = new Signal(new List<float>() { 1, 2, 3, 8 }, new List<int>() { -1, 0, 1, 2 }, false);
            f.Run();
            // {8, 3, 2, 1}
            // -2 -1, 0, 1
            s.InputSignal = f.OutputFoldedSignal;
            s.ShiftingValue = 2;
            s.Run();
            // {8, 3, 2, 1}
            //  0, 1, 2, 3
            var f2 = new Folder();
            f2.InputSignal = s.OutputShiftedSignal;
            f2.Run();
            // {1, 2, 3, 8}
            // -3, -2, -1, 0
            var s2 = new Shifter();
            s2.InputSignal = f2.OutputFoldedSignal;
            s2.ShiftingValue = 2;
            s2.Run();
            // {1, 2, 3, 8}
            // -5, -4, -3, -2

            actualOutputSignal = s2.OutputShiftedSignal;
            expectedOutputSignal = new Signal(new List<float>(){1, 2, 3, 8}, new List<int>(){-5, -4, -3, -2}, false);
        }
    }
}
