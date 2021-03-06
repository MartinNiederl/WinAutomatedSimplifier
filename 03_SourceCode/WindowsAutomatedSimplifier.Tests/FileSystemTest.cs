// <copyright file="FileSystemTest.cs">Copyright ©  2016</copyright>
using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WindowsAutomatedSimplifier.FileSystem;

namespace WindowsAutomatedSimplifier.FileSystem.Tests
{
    /// <summary>Diese Klasse enthält parametrisierte Komponententests für FileSystemLogic.</summary>
    [PexClass(typeof(global::WindowsAutomatedSimplifier.FileSystem.FileSystemLogic))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class FileSystemTest
    {
        /// <summary>Test-Stub für DeleteEmptyDirectories(String, Boolean)</summary>
        [PexMethod]
        internal void DeleteEmptyDirectoriesTest(string directoryPath, bool topDirectoryOnly)
        {
            // TODO: Assertionen zu Methode FileSystemTest.DeleteEmptyDirectoriesTest(String, Boolean) hinzufügen
        }
    }
}
