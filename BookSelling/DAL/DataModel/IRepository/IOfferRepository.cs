using DAL.Enums;
using DAL.Models;

namespace DAL.IRepository
{
    public interface IOfferRepository
    {
        OfferStatus? GetOfferStatus(int bookId);
        OfferStatus? UpdateOfferStatus(BookModel model);
    }
}