namespace LetsGame.Common.Data.Entities.Core;

public interface IEntityMetaData
{
    DateTime DateAdded { get; set; }
    DateTime LastModified { get; set; }
}
