namespace FakeStockProxy.UnitTests.Fixtures;

public sealed class FsDataWrapper
{
    private const int fsStockRequestItemsCount = 5;
    public FsTestDatasource fsTestDatasource { get; init; }
    public IFsStockRequestRepositoryMock fsStockRequestRepositoryMock { get; init; }

    public FsDataWrapper() 
    {
        fsTestDatasource = new FsTestDatasource(fsStockRequestItemsCount);
        fsStockRequestRepositoryMock = new IFsStockRequestRepositoryMock(fsTestDatasource);
    }

}
