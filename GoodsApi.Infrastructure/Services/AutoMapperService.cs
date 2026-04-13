using AutoMapper;

namespace GoodsApi.Infrastructure.Services;

public static class AutoMapperService
{
    private static IMapper _mapper = null!;
    
    public static void Initialize(IMapper mapper)
    {
        _mapper = mapper;
    }
    
    public static TDestination Convert<TSource, TDestination>(this TSource source) where TSource : class
    {
        return _mapper.Map<TSource, TDestination>(source);
    }
    
    public static List<TDestination> Convert<TSource, TDestination>(this List<TSource> source) where TSource : class
    {
        List<TDestination> destionation = new List<TDestination>();
        foreach (var item in source)
        {
            destionation.Add(item.Convert<TSource, TDestination>());
        }

        return destionation;
    }
}