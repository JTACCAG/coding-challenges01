using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace Api.Application.Validators
{
    public class MongoObjectIdArrayAttribute : ValidationAttribute
    {
        public bool IsRequired { get; set; } = true;

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            // 🔹 Si es opcional y no viene, es válido
            if (!IsRequired && value is null)
                return ValidationResult.Success;

            // 🔹 Si es requerido y no viene
            if (IsRequired && value is null)
                return new ValidationResult($"{validationContext.MemberName} es requerido");

            if (value is not string[] ids)
                return new ValidationResult("Formato inválido");

            if (IsRequired && ids.Length == 0)
                return new ValidationResult($"{validationContext.MemberName} no puede estar vacío");

            foreach (var id in ids)
            {
                if (string.IsNullOrWhiteSpace(id))
                    return new ValidationResult("No puede contener valores vacíos");

                if (!ObjectId.TryParse(id, out _))
                    return new ValidationResult($"'{id}' no es un MongoDB ObjectId válido");
            }

            return ValidationResult.Success;
        }
    }
}
