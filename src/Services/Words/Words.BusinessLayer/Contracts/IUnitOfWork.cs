namespace Words.BusinessLayer.Contracts
{
    public interface IUnitOfWork
    {
        public ILanguageRepository Languages { get; }
        public IUserRepository Users { get; }
        public IUserWordRepository UserWords { get; }
        public IUserWordTypeRepository UserWordTypes { get; }
        public IWordTypeRepository WordTypes { get; }
    }
}
