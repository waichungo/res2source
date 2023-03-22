#include <vector>
using std::vector;
namespace resource
{
    class Resource
    {
    public:
        static std::vector<unsigned char> GetFont()
        {
            std::vector<unsigned char> result;
            const char buffer[1024] = {};
            result.resize(sizeof(buffer));
            memcpy(&result[0], buffer, sizeof(buffer));
            return result;
        }
    };
}