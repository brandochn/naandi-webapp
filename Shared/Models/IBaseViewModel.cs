using System;

namespace Naandi.Shared.Models
{
    public interface  IBaseViewModel
    {
        bool IsValid(object value);
    }
}