// <copyright file="MessageBoxTest.cs">Copyright ©  2017</copyright>
using System;
using System.Windows;
using CustomMessageBox;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CustomMessageBox.Tests
{
    /// <summary>Diese Klasse enthält parametrisierte Komponententests für MessageBox.</summary>
    [PexClass(typeof(MessageBox))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClassAttribute]
    public partial class MessageBoxTest
    {
        /// <summary>Test-Stub für .ctor()</summary>
        [PexMethod]
        public MessageBox ConstructorTest()
        {
            MessageBox target = new MessageBox();
            return target;
            // TODO: Assertionen zu Methode MessageBoxTest.ConstructorTest() hinzufügen
        }

        /// <summary>Test-Stub für Show(String, String, MessageBoxType)</summary>
        [PexMethod]
        public MessageBoxResult ShowTest(
            string caption,
            string msg,
            MessageBoxType type
        )
        {
            MessageBoxResult result = MessageBox.Show(caption, msg, type);
            return result;
            // TODO: Assertionen zu Methode MessageBoxTest.ShowTest(String, String, MessageBoxType) hinzufügen
        }

        /// <summary>Test-Stub für Show(String, MessageBoxType)</summary>
        [PexMethod]
        public MessageBoxResult ShowTest01(string msg, MessageBoxType type)
        {
            MessageBoxResult result = MessageBox.Show(msg, type);
            return result;
            // TODO: Assertionen zu Methode MessageBoxTest.ShowTest01(String, MessageBoxType) hinzufügen
        }

        /// <summary>Test-Stub für Show(String)</summary>
        [PexMethod]
        public MessageBoxResult ShowTest02(string msg)
        {
            MessageBoxResult result = MessageBox.Show(msg);
            return result;
            // TODO: Assertionen zu Methode MessageBoxTest.ShowTest02(String) hinzufügen
        }

        /// <summary>Test-Stub für Show(String, String)</summary>
        [PexMethod]
        public MessageBoxResult ShowTest03(string caption, string text)
        {
            MessageBoxResult result = MessageBox.Show(caption, text);
            return result;
            // TODO: Assertionen zu Methode MessageBoxTest.ShowTest03(String, String) hinzufügen
        }

        /// <summary>Test-Stub für Show(String, String, MessageBoxButton)</summary>
        [PexMethod]
        public MessageBoxResult ShowTest04(
            string caption,
            string text,
            MessageBoxButton button
        )
        {
            MessageBoxResult result = MessageBox.Show(caption, text, button);
            return result;
            // TODO: Assertionen zu Methode MessageBoxTest.ShowTest04(String, String, MessageBoxButton) hinzufügen
        }
    }
}
