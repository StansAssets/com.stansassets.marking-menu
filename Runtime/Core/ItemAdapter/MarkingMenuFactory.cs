using System;
using System.Collections.Generic;
using System.Reflection;

namespace StansAssets.MarkingMenuB
{
    class MarkingMenuFactory
    {
        public class MMAdapterAttribute : System.Attribute
        {
            public Type Type;

            public MMAdapterAttribute(Type type) {
                this.Type = type;
            }
        }

        internal class MMAdapterFactory
        {
            static void Reset() {
                ResetAdapters();
            }

            static readonly Dictionary<Type, Type> s_Adapters = new Dictionary<Type, Type>();
            static readonly Dictionary<int, IMarkingMenuItemAdapter> FoundAdapters = new Dictionary<int, IMarkingMenuItemAdapter>();

            static IEnumerable<KeyValuePair<Type, Type>> Adapters => s_Adapters;

            internal static void CreateAdapterFactory() {
                s_Adapters.Clear();
                foreach (var type in Assembly.GetExecutingAssembly().GetTypes()) {
                    var attributes = type.GetCustomAttributes(typeof(MMAdapterAttribute), false);
                    if (attributes.Length > 0) {
                        var adapter = (MMAdapterAttribute)attributes[0];
                        if (adapter != null) {
                            s_Adapters.Add(adapter.Type, type);
                        }
                    }
                }
            }

            internal static void ResetAdapters() {
                // Cleanup adapters before collection clear
                foreach (var pair in FoundAdapters)
                {
                    pair.Value.Disable();
                }
                FoundAdapters.Clear();
            }

            static IMarkingMenuItemAdapter CreateAdapter(IMarkingMenuItem item) {
                if (s_Adapters.Count == 0) {
                    CreateAdapterFactory();
                }

                Type adapterType = null;
                adapterType = GetAdapterTypeRecursively(rootType: item.GetType(),
                    baseType: typeof(IMarkingMenuItem),
                    typesDict: s_Adapters,
                    baseAdapterType: typeof(IMarkingMenuItemAdapter));

                if (adapterType != null) {
                    var adapter = Activator.CreateInstance(adapterType, new object[] { item }) as IMarkingMenuItemAdapter;
                    int id = item.Id;
                    if (!FoundAdapters.ContainsKey(id))
                        FoundAdapters.Add(id, adapter);

                    return adapter;
                }

                //Default adapter
                if (s_Adapters.TryGetValue(typeof(IMarkingMenuItem), out adapterType)) {
                    var adapter = Activator.CreateInstance(adapterType, new object[] { item }) as IMarkingMenuItemAdapter;
                    int id = item.Id;
                    if (!FoundAdapters.ContainsKey(id))
                        FoundAdapters.Add(id, adapter);

                    return adapter;
                }

                return null;
            }

            internal static IMarkingMenuItemAdapter GetAdapter(IMarkingMenuItem item) {
                if (item == null) return null;

                int id = item.Id;
                if (FoundAdapters.ContainsKey(id)) {
                    //MoonDebug.Log("GetAdapter() found " + component.name);
                    return FoundAdapters[id];
                }

                //MoonDebug.Log("GetAdapter() create " + component.name);
                return CreateAdapter(item);
            }

            static Type GetAdapterTypeFor(Type type) {
                s_Adapters.TryGetValue(type, out var adapterType);
                return adapterType;
            }

            static Type GetAdapterTypeRecursively(Type rootType, Type baseType, IReadOnlyDictionary<Type, Type> typesDict, Type baseAdapterType) {
                if (rootType == null || baseType == null) return null;

                //return base type
                if (rootType == baseType) {
                    return baseAdapterType;
                }
                //success, we have adapter for this rootType

                return typesDict.ContainsKey(rootType) ? typesDict[rootType] : GetAdapterTypeRecursively(rootType.BaseType, baseType, typesDict, baseAdapterType);
            }
        }
    }
}
