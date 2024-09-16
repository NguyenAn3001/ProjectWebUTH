///096205002758_TranDongHil
#include <iostream>
#include <fstream>
#include <string>
using namespace std;
class FileProcess {
public:
    void readFile(string path);
};
void FileProcess::readFile(string path) {
    ifstream readFile(path);
    if (readFile.is_open()) {
        string str;
        int line = 0;
        cout << "096205002758_TranDongHil" << endl;
        cout << "\ndata.txt:" << endl;
        while (getline(readFile, str)) {
            cout << str << endl;
            line++;
        }
        cout << "\nNumber of lines: " << line << endl;
        readFile.close();

        ifstream readFile(path);
        if (readFile.is_open()) {
            cout << "Lines starting with '-': " << endl;
            while (getline(readFile, str)) {
                if (str.compare(0, 1, "-") == 0) {
                    cout << str << endl;
                }
            }
            readFile.close();
        }
    }
}
int main() {
    FileProcess f;
    f.readFile("data.txt");
    return 0;
}
