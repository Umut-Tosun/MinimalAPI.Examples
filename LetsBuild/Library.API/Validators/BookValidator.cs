using FluentValidation;
using Library.API.Models;

namespace Library.API.Validators
{
    public sealed class BookValidator : AbstractValidator<Book>
    {
        public BookValidator() 
        {
            RuleFor(p => p.Isbn)
                .NotEmpty().WithMessage("ISBN boş olamaz")
                .Length(10, 13).WithMessage("ISBN 10 veya 13 karakter olmalı")
                .Matches(@"^[\d-]+$").WithMessage("ISBN sadece rakam ve tire içerebilir");

         
            RuleFor(p => p.Title)
                .NotEmpty().WithMessage("Başlık boş olamaz")
                .MinimumLength(3).WithMessage("Başlık en az 3 karakter olmalı")
                .MaximumLength(200).WithMessage("Başlık en fazla 200 karakter olabilir");

           
            RuleFor(p => p.ShortDescription)
                .NotEmpty().WithMessage("Kısa açıklama boş olamaz")
                .MinimumLength(10).WithMessage("Kısa açıklama en az 10 karakter olmalı")
                .MaximumLength(500).WithMessage("Kısa açıklama en fazla 500 karakter olabilir");

            RuleFor(p => p.PageCount)
                .NotEmpty().WithMessage("Sayfa sayısı boş olamaz")
                .GreaterThan(0).WithMessage("Sayfa sayısı 0'dan büyük olmalı")
                .LessThanOrEqualTo(10000).WithMessage("Sayfa sayısı 10000'den küçük olmalı");

         
          
            RuleFor(p => p.PublishDate)
                .NotEmpty().WithMessage("Yayın tarihi boş olamaz")
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Yayın tarihi gelecek tarih olamaz")
                .GreaterThan(new DateTime(1800, 1, 1)).WithMessage("Yayın tarihi 1800'den sonra olmalı");


        }


    }
}
