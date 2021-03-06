// <copyright file="FontPickerTest.cs">Copyright ©  2016</copyright>

using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WindowsAutomatedSimplifier.ChangeFont;

namespace WindowsAutomatedSimplifier.ChangeFont.Tests
{
    [TestClass]
    [PexClass(typeof(FontPicker))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class FontPickerTest
    {

        [PexMethod]
        [PexAllowedException(typeof(TypeInitializationException))]
        public FontPicker Constructor()
        {
            FontPicker target = new FontPicker();
            return target;
            // TODO: Assertionen zu Methode FontPickerTest.Constructor() hinzufügen
        }
    }
}
