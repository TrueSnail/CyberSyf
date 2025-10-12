using E_Book_Store.Controllers;
using E_Book_Store.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace E_Book_Tests;

public class EBooksControllerTests
{
    [Fact]
    public void BuyTest()
    {
        //Arrange
        const string USER_ID = "123";
        const string EBOOK_ID = "235";

        var store = new Mock<IUserStore<IdentityUser>>();
        var mockUserManager = new Mock<UserManager<IdentityUser>>(store.Object, null!, null!, null!, null!, null!, null!, null!, null!);
        mockUserManager.Setup(manager => manager.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(USER_ID);

        var mockEbookService = new Mock<IEBooksService>();
        mockEbookService.Setup(service => service.Buy(It.IsAny<string>(), It.IsAny<string>())).Callback(CheckEbookServiceBuy);

        var controller = new EBooksController(mockEbookService.Object, null!, null!, null!, mockUserManager.Object);

        //Act
        var result = controller.Buy(EBOOK_ID);

        //Assert
        Assert.IsType<RedirectToActionResult>(result);
        void CheckEbookServiceBuy(string ebookId, string userId)
        {
            Assert.Equal(USER_ID, userId);
            Assert.Equal(EBOOK_ID, ebookId);
        }

    }
}
