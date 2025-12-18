using Common.Entities;
using System;
using System.Collections.Generic;

namespace API.Infrastructure.ResponseDTOs.Shared;

public class BaseGetResponse<E>
    where E : BaseEntity
{
    public List<E> Items { get; set; }
    public PagerResponse Pager { get; set; }
    public string OrderBy { get; set; }
    public bool SortAsc { get; set; }
}
