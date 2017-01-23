using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.TableInterfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelRepository;
using ModelRepository.Internal;
using ModelRepository.Internal.ModelHelpers;
using ModelRepository.Internal.Models;
using ModelRepository.ModelInterfaces;
using ModelUtilities;
using Moq;
using FluentAssertions;
using StructureMap;

namespace ModelRepositoryTest.Tests.Models
{
  [TestClass]
  public class TestVoiceMailModels
  {
    //requires the following packages    
    
    [TestMethod]
    public void TestVoiceMailGetsVoiceFolders()
    {
      //Arrange
      var repo = new Mock<IRepositoryWithDelete>();
      var randomNumber = new Random().Next();
       var randomName = new Random().Next().ToString(CultureInfo.InvariantCulture);
      var mockVoiceMessage = new Mock<IVoiceMessage>();
      mockVoiceMessage.Setup(c => c.MailBox.Id).Returns(randomNumber);
      mockVoiceMessage.Setup(c => c.Folder.FolderName).Returns(randomName);

      var mockIAstVoice = new Mock<IAstVoicemail>();
      mockIAstVoice.Setup(c => c.Id).Returns(randomNumber);
      var listVm = new List<IVoiceMessage> { mockVoiceMessage.Object };

      repo.Setup(r => r.GetList<IVoiceMessage>()).Returns(listVm);
      var folder = new Mock<IFolderInformation>();
      folder.Setup(f => f.FolderName).Returns(randomName);
      var mockFolderList = new List<IFolderInformation> { folder.Object };

      var mockVoiceMessageManager = new Mock<IMessageFolderManager>();
     
      mockVoiceMessageManager.Setup(v => v.GetAllMessageFolders()).Returns(mockFolderList);

      //Act
      var voice = new VoiceMail(mockIAstVoice.Object, repo.Object, mockVoiceMessageManager.Object);
      var listOfVoiceFolders = voice.MessageFolders;
      //Assert

      repo.Verify(r => r.GetList<IVoiceMessage>(), Times.Exactly(1));
      listOfVoiceFolders.Count().Should().Be(mockFolderList.Count);
    }

    [TestMethod]
    public void TestVoiceMessageCallerIdIsExtension()
    {
      //Arrange
      var repo = new Mock<IRepositoryWithDelete>();
      var randomNumber = new Random().Next(0,1000).ToString(CultureInfo.InvariantCulture);

      var mockIAstVoice = new Mock<IAstVoiceMessage>();
      mockIAstVoice.Setup(c => c.CallerId).Returns(randomNumber);

      var mockAstAs= new Mock<IAsteriskAudioStream>();

      var mockExten = new Mock<IExtension>();
      mockExten.Setup(e => e.Number).Returns(randomNumber);
      mockExten.SetupProperty(e => e.FirstName, randomNumber);
      mockExten.SetupProperty(e => e.LastName, randomNumber);

      var listExten = new List<IExtension> {mockExten.Object};
      repo.Setup(r => r.GetList<IExtension>()).Returns(listExten);

      //Act
      var voice = new VoiceMessage(mockIAstVoice.Object, repo.Object, mockAstAs.Object);
      var callerId = voice.CallerId;
      //Assert

      repo.Verify(r => r.GetList<IExtension>(), Times.Exactly(1));
      callerId.Should().BeEquivalentTo(string.Format("{0} {1}", mockExten.Object.FirstName, mockExten.Object.LastName));
      repo.Verify(r=>r.GetList<IExtension>(), Times.Exactly(1));
    }

    [TestMethod]
    public void TestVoiceMessageCallerIdIsKnownNumber()
    {
      //Arrange
      var repo = new Mock<IRepositoryWithDelete>();
      var randomNumber = new Random().Next(0, 1000).ToString(CultureInfo.InvariantCulture);

      var mockIAstVoice = new Mock<IAstVoiceMessage>();
      mockIAstVoice.Setup(c => c.CallerId).Returns(randomNumber);

      var mockAstAs = new Mock<IAsteriskAudioStream>();

      var mockKnown = new Mock<IKnownNumber>();
      mockKnown.Setup(e => e.Number).Returns(randomNumber);
      mockKnown.SetupProperty(e => e.Description, randomNumber);

      var listKnown = new List<IKnownNumber> { mockKnown.Object };
      repo.Setup(r => r.GetList<IKnownNumber>()).Returns(listKnown);

      //Act
      var voice = new VoiceMessage(mockIAstVoice.Object, repo.Object, mockAstAs.Object);
      var callerId = voice.CallerId;
      //Assert

      repo.Verify(r => r.GetList<IExtension>(), Times.Exactly(1));
      callerId.Should().BeEquivalentTo(string.Format("{0}", mockKnown.Object.Description));
      repo.Verify(r => r.GetList<IKnownNumber>(), Times.Exactly(1));
    }

    [TestMethod]
    public void TestVoiceMessageFolderGetsItemsForFolder()
    {
      //Arrange
      var rand = new Random();
      var randomAmount1 = rand.Next(1, 10);
      var randomAmount2 = rand.Next(1, 10);
      var folderName = rand.Next().ToString(CultureInfo.InvariantCulture);

      var listOfMesages = new List<IVoiceMessage>();

      for (int i = 0; i < randomAmount1; i++)
      {
        var mockVoiceMessage = new Mock<IVoiceMessage>();
        mockVoiceMessage.Setup(v => v.Folder.FolderName).Returns(folderName);
        listOfMesages.Add(mockVoiceMessage.Object);
      }

      for (int i = 0; i < randomAmount2; i++)
      {
        var mockVoiceMessage = new Mock<IVoiceMessage>();
        mockVoiceMessage.Setup(v => v.Folder.FolderName).Returns("");
        listOfMesages.Add(mockVoiceMessage.Object);
      }
     
      //Act
      var vMf = new VoiceMessageFolder(listOfMesages, folderName, 1);
      var listOfm = vMf.FolderMessages;

      //Assert
      listOfm.Count().Should().Be(randomAmount1);
    }
  }
}
