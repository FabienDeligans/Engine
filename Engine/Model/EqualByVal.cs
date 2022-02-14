namespace Engine.Model
{
    public class EqualByVal
    {
        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            foreach (var propertyOfObj in obj.GetType().GetProperties())
            {
                var prop = GetType().GetProperty(propertyOfObj.Name);
                if (prop?.GetValue(this)?.ToString() != propertyOfObj.GetValue(obj)?.ToString())
                {
                    return false;
                }
            }
            return true;
        }
    }
}
