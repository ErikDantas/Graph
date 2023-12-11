using System;
using System.Collections.Generic;
using System.Linq;

namespace Graph
{
    public interface IGraph<T>
    {
        List<List<string>> RoutesBetween(T source, T target);
    }

    public class Graph<T> : IGraph<T>
    {
        private IEnumerable<ILink<T>> _links;
        private List<string> result = new List<string>();
        private List<List<string>> routes = new List<List<string>>();
        public Graph(IEnumerable<ILink<T>> links)
        {
            _links = links;
        }

        public List<List<string>> RoutesBetween(T source, T target)
        {
            try
            { 
                var sourceTarget = RecursiveRoutesBetween(source, target);
                if(!result.Contains(source.ToString()) && result.Contains(target.ToString())){
                    RoutesBetween(sourceTarget, target);
                }
                else
                {
                    routes.Add(result.ToList());
                    result = new List<string>();
                }
                RoutesBetween(source, target);
            }
            catch
            {
                return routes;
            }
            return routes;
        }

        private T RecursiveRoutesBetween(T source, T target) 
        {
            bool foundRoute = false;
            foreach (var link in _links)
            {
                bool alreadyFound = false;

                if (link.Source.Equals(source))
                {
                    foreach (var route in routes)
                    {
                        if (route.Contains(link.Target.ToString()) && !link.Target.Equals(target))
                        {
                            alreadyFound = true;
                            break;
                        }
                    }
                    if (alreadyFound)
                    {
                        continue;
                    }
                    foundRoute = true;

                    if (!result.Contains(link.Source.ToString()))
                    {
                        result.Add(link.Source.ToString());
                        
                    }
                    if (!result.Contains(link.Target.ToString()))
                    {
                        result.Add(link.Target.ToString());
                        source = link.Target;

                    }
                }
            }
            if (!foundRoute)
            {
                throw new Exception("Not found");
            }
            return source;
        }
    }
}
