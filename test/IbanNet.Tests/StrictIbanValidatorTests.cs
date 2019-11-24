﻿using System.Collections;
using FluentAssertions;
using NUnit.Framework;

namespace IbanNet
{
	[TestFixtureSource(nameof(ValidatorMethodTestCases))]
	internal class StrictIbanValidatorTests : IbanValidatorTests
	{
		public StrictIbanValidatorTests(string fixtureName, IbanValidator validator)
			: base(fixtureName, validator)
		{
		}

		public static IEnumerable ValidatorMethodTestCases()
		{
			yield return new object[] { "DefaultStrict", new IbanValidator() };
		}

		[TestCase("NL91ABNA041716430A")]
		[TestCase("NO938601111794A")]
		public void When_validating_iban_with_invalid_structure_should_not_validate(string ibanWithInvalidStructure)
		{
			// Act
			ValidationResult actual = Validator.Validate(ibanWithInvalidStructure);

			// Assert
			actual.Should().BeEquivalentTo(new ValidationResult
			{
				Value = ibanWithInvalidStructure,
				Result = IbanValidationResult.InvalidStructure,
				Country = CountryValidationSupport.SupportedCountries[ibanWithInvalidStructure.Substring(0, 2)]
			});
		}

		[TestCase("NL91abna0417164300", IbanValidationResult.InvalidStructure, Description = "A region that requires bank details to be upper case")]
		[TestCase("MT84MALT011000012345mtlcast001S", IbanValidationResult.Valid, Description = "A region that allows bank details to be lower case")]
		public void When_validating_iban_with_invalid_case_should_not_be_valid(string lowerIban, IbanValidationResult expectedResult)
		{
			// Act
			ValidationResult actual = Validator.Validate(lowerIban);

			// Assert
			actual.Result.Should().Be(expectedResult);
		}
	}
}
