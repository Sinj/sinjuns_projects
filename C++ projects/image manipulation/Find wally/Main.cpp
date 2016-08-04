#include <sstream> // stringstream
#include <iostream> // cout, cerr
#include <fstream> // ifstream
#include <istream>
#include "Matrix.h"


using namespace std;


// Reads .txt file 
double* readTXT(char *fileName, int sizeR, int sizeC);

//write to .pgm image
void WritePGM(char *filename, double *data, int sizeR, int sizeC, int Q);
double NC(Matrix, Matrix);
Matrix& subMean(Matrix&);

int main(){

	//M = Rows and N = columns in the image,
	//Cluttered_scene, M = 768, N = 1024, Wally_grey, M = 49, N =  36

	int M, N,count=0;
	//store both images data
	double* input_data_cluttered = 0;
	double* input_data_wally = 0;
	double nc = 0.0, lastnc = 0.0;
	cout << endl;
	cout << "-------------- Reading Data from text files --------------" << endl;
	cout << endl;

	// input text filepath
	char* FileName = "C:\\Users\\Sinjun\\Desktop\\Assignment 1 - files\\Task 2files\\Cluttered_scene.txt";
	input_data_cluttered = readTXT(FileName, 768, 1024);
	FileName = "C:\\Users\\Sinjun\\Desktop\\Assignment 1 - files\\Task 2files\\Wally_grey.txt";
	input_data_wally = readTXT(FileName, 49, 36);

	Matrix wally(49, 36, input_data_wally);
	Matrix wholeimg(768, 1024, input_data_cluttered);
	cout << endl;
	cout << "->>>>>>>>>>-        Reading Done         ->>>>>>>>>>-" << endl;
	cout << endl;
	

	for (int i = 0; i < 719; i++){
		for (int j = 0; j < 998; j++)
		{
			Matrix cluttered = wholeimg.getblock(0 + i, i + 49, 0 + j, j + 36); // for comparing agaist wally
			nc = NC(wally, cluttered);//send both wally and cluttered to get comapered agasit
			if (nc > lastnc)//check the NC is greater then the last
			{
				//save the location of the most simlar 
				M = i;
				N = j;
				lastnc = nc;
			}
		}
	}
	cout << "-<<<<<<<<<<-        Compared Data        -<<<<<<<<<<-" << endl;
	cout << endl;
	
	//set the found loction to a black block
	for (int i = M; i <= M+49; i++)
	for (int j = N; j <= N+36; j++)
	{
		wholeimg.set(i, j, 0);
	}
	
	cout << "-<<<<<<<<<<-        Image Changed        -<<<<<<<<<<-" << endl;
	cout << endl;
	
	
	//save the image back into data
	for (int i = 0; i < 768; i++)
	for (int j = 0; j < 1024; j++)
	{
		input_data_cluttered[count] = wholeimg.get(i, j);
		count++;
	}
	
	// outputs image path(where the image will be)
	char* outputFileName = "C:\\Users\\Sinjun\\Desktop\\Assignment 1 - files\\Task 2files\\Cluttered_scene.pgm";
	// Use Q = 255 for greyscale images and 1 for binary images.
	int Q = 255;
	WritePGM(outputFileName, input_data_cluttered, 768, 1024, Q);
	outputFileName = "C:\\Users\\Sinjun\\Desktop\\Assignment 1 - files\\Task 2files\\Wally_grey.pgm";
	WritePGM(outputFileName, input_data_wally, 49, 36, Q);

	cout << "-<<<<<<<<<<-        Image Rebuilt        -<<<<<<<<<<-" << endl;
	cout << endl;
	delete[] input_data_cluttered;
	delete[] input_data_wally;

	cout << "-------------------- Done  --------------------" << endl;
	return 0;
}



// Read .txt file with image of size RxC, and convert to an array of doubles
double* readTXT(char *fileName, int sizeR, int sizeC)
{
	double* data = new double[sizeR*sizeC];
	int i = 0;
	ifstream myfile(fileName);
	if (myfile.is_open())
	{

		while (myfile.good())
		{
			if (i > sizeR*sizeC - 1) break;
			myfile >> *(data + i);
			// cout << *(data+i) << ' '; // This line display the converted data on the screen, you may comment it out. 
			i++;
		}
		myfile.close();
	}

	else cout << "Unable to open file";
	//cout << i;

	return data;
}

// convert data from double to .pgm stored in filename
void WritePGM(char *filename, double *data, int sizeR, int sizeC, int Q)
{

	int i;
	unsigned char *image;
	ofstream myfile;

	image = (unsigned char *) new unsigned char[sizeR*sizeC];

	// convert the integer values to unsigned char
	for (i = 0; i < sizeR*sizeC; i++)
		image[i] = (unsigned char)data[i];

	myfile.open(filename, ios::out | ios::binary | ios::trunc);

	if (!myfile) {
		cout << "Can't open file: " << filename << endl;
		exit(1);
	}

	myfile << "P5" << endl;
	myfile << sizeC << " " << sizeR << endl;
	myfile << Q << endl;

	myfile.write(reinterpret_cast<char *>(image), (sizeR*sizeC)*sizeof(unsigned char));

	if (myfile.fail()) {
		cout << "Can't write image " << filename << endl;
		exit(0);
	}

	myfile.close();

	delete[] image;
}

//Normalised Correlation
double NC(Matrix I, Matrix T)
{
	double nc = 0.0, numerator = 0.0, denominator = 0.0;
	
	//get th mean and then sub the mean from the elements for both matrixs
	subMean(I);
	subMean(T);

	//get the numerator
	for (int i = 0; i <= 49; i++)
	for (int j = 0; j <= 36; j++)
	{
		numerator += I.get(i, j)*T.get(i, j);
	}

	//get the denominator
	for (int i = 0; i <= 49; i++)
	for (int j = 0; j <= 36; j++)
	{
		denominator += (I.get(i, j)*I.get(i, j))*(T.get(i, j)*T.get(i, j));
	}
	//sqaure the denominator
	denominator *= denominator;

	nc = numerator / denominator;
	
	//return the Normalised Correlation value 
	return  nc;
}

Matrix& subMean(Matrix &X)
{
	double mean = 0;
	//get mean
	for (int i = 0; i <= 49; i++)
	for (int j = 0; j <= 36; j++)
	{
		mean += X.get(i, j);
	}

	mean /= (49 * 36);

	//take mean
	for (int i = 0; i <= 49; i++)
	for (int j = 0; j <= 36; j++)
	{
		X.set(i, j, X.get(i, j) - mean);
	}
	return X;

}

