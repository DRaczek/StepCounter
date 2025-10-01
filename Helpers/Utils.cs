using System.ComponentModel;

namespace StepCounter.Helpers
{
    public class Utils
    {
        public void ForwardProperty(INotifyPropertyChanged source, string propertyName, Action<string> notifyTarget)
        {
            source.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == propertyName)
                    notifyTarget(propertyName);
            };
        }
        public void ForwardProperties(INotifyPropertyChanged source, IEnumerable<string> propertyNames, Action<string> notifyTarget)
        {
            foreach(string property in propertyNames)
            {
                ForwardProperty(source, property, notifyTarget);
            }
        }
    }
}
