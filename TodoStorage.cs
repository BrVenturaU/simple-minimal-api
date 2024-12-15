using BackgroundDemo.Models;
using System.Collections.Concurrent;

namespace BackgroundDemo
{
    public class TodoStorage
    {
        public ConcurrentDictionary<Guid, IList<Todo>> Todos { get; set; } = new();
    }
}
