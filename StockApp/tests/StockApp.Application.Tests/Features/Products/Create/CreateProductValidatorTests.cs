using FluentValidation.TestHelper;
using Shouldly;
using StockApp.Application.Features.Products.Create;

namespace StockApp.Application.Tests.Features.Products.Create;

public class CreateProductValidatorTests
{
    private readonly CreateProductValidator _validator =
        new();

    [Fact]
    public void Should_Have_Error_When_Name_Is_Empty()
    {
        // Arrange
        var command = new CreateProductCommand("", 10);

        // Act
        var result =
            _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Command_Is_Valid()
    {
        // Arrange
        var command = new CreateProductCommand(
            "Notebook",
            5000);

        // Act
        var result =
            _validator.TestValidate(command);

        // Assert
        result.IsValid.ShouldBeTrue();
    }
}