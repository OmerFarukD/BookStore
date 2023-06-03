﻿using System.Dynamic;
using Entities.Models;

namespace Services.Abstracts;

public interface IDataShaper<T>
{
    IEnumerable<ShapedEntity> ShapeData(IEnumerable<T> entities,string fieldsString);
    ShapedEntity ShapeData(T entity,string fieldsString);
}