using MediatR;

public class GetAllCategoriesHandler
    : IRequestHandler<GetAllCategoriesQuery, List<CategoryResponseDto>>
{
    private readonly ICategoryQueryRepository _repo;

    public GetAllCategoriesHandler(ICategoryQueryRepository repo)
    {
        _repo = repo;
    }

    public async Task<List<CategoryResponseDto>> Handle(
        GetAllCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        var categories = (await _repo.QueryAsync<CategoryResponseDto>(
            "inventory.sp_GetAllCategories"))
            .ToList();

 
        var lookup = categories.ToDictionary(x => x.Id);
        var roots = new List<CategoryResponseDto>();

        foreach (var cat in categories)
        {
            if (cat.ParentId.HasValue && lookup.ContainsKey(cat.ParentId.Value))
                lookup[cat.ParentId.Value].Children.Add(cat);
            else
                roots.Add(cat);
        }

        return roots;
    }
}