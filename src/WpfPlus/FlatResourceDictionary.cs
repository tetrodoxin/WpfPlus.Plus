using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace WpfPlus
{
    /// <summary>
    /// resource dictionary that takes all resources of a given dictionary,
    /// even if they are embedded in nested dictionaries (tree traversal),
    /// THUS: flattening nested dictionary structure.
    /// </summary>
    /// <seealso cref="System.Windows.ResourceDictionary" />
    public class FlatResourceDictionary : ResourceDictionary
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FlatResourceDictionary"/> class.
        /// </summary>
        /// <param name="resourceUri">An URI referencing an existing resource dictionary.</param>
        /// <param name="suppressErrors">if set to <c>true</c>, occuring exceptions are ignored and an empty <see cref="FlatResourceDictionary"/> is returned.</param>
        public FlatResourceDictionary(string resourceUri, bool suppressErrors = true)
            : this(getResourceDictionary(resourceUri, suppressErrors))
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FlatResourceDictionary"/> class.
        /// </summary>
        /// <param name="resourceEntries">A sequence of dictionary entries.</param>
        public FlatResourceDictionary(IEnumerable<DictionaryEntry> resourceEntries)
        {
            foreach (var entry in resourceEntries)
                this[entry.Key] = entry.Value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FlatResourceDictionary"/> class.
        /// </summary>
        /// <param name="resrcDict">An existing dictionary.</param>
        public FlatResourceDictionary(ResourceDictionary resrcDict)
            : this(flattenResourceDictionary(resrcDict))
        { }

        private static ResourceDictionary getResourceDictionary(string resourceUri, bool suppressErrors)
        {
            try
            {
                return new ResourceDictionary() { Source = new Uri(resourceUri, UriKind.Relative) };
            }
            catch
            {
                if (suppressErrors)
                    return new ResourceDictionary();
                else
                    throw;
            }
        }

        private static IEnumerable<DictionaryEntry> flattenResourceDictionary(ResourceDictionary resrcDict)
            => resrcDict.MergedDictionaries
                .SelectMany(dict => flattenResourceDictionary(dict))
                .Concat(resrcDict.Keys.Cast<object>()
                    .Select(key => new DictionaryEntry(key, resrcDict[key])));
    }
}