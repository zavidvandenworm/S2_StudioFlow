using Domain.Entities;
using Infrastructure.Helpers;

namespace Tests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void FillClass_WithSuppliedParameters_FillsCorrectly()
    {
        // arrange
        var profile = new Profile()
        {
            UserId = -1,
            DisplayName = "Test",
            Biography = "Test Biography"
        };
        var values = new Dictionary<string, object>()
        {
            { "Email", "mail" },
            { "Username", "user" },
            { "PasswordHash", "hash" },
            {
                "Profile", profile
            }
        };
        
        // act
        User user = ClassFiller.FillClass<User>(values);
        
        // assert
        Assert.That(user.Email, Is.EqualTo("mail"));
        Assert.That(user.Username, Is.EqualTo("user"));
        Assert.That(user.PasswordHash, Is.EqualTo("hash"));
        Assert.That(user.Profile, Is.EqualTo(profile));
    }
}