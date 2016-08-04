#include <sstream> // stringstream
#include <iostream> // cout, cerr
#include <fstream> // ifstream
#include <istream>
#include "Matrix.h"
#include "BinaryImage.h"

using namespace std;


//Reads .txt file 
double* readTXT(char *fileName, int sizeR, int sizeC);

//Makes .pgm image file
void WritePGM(char *filename, double *data, int sizeR, int sizeC, int Q);

//bool to check if txt files have been read
bool File_has_read = true;

int main(){
	
	//M = Rows and N = columns in the image,
	int M = 512; int N = 512;

	//threshhold that will be used to remove the noise from image
	double thresh = 170;
	
	//store both image data
	double* input_data_noise = 0;
	double* input_data_shuffle = 0;
	int count = 0;

	cout << endl;
	cout << "-------------- Reading Data from text file --------------" << endl << endl;

	// input text filepath
	char* inputFileName = "C:\\Users\\Sinjun\\Desktop\\Assignment 1 - files\\Task1files\\logo_with_noise.txt";
	input_data_noise = readTXT(inputFileName, M, N);

	inputFileName = "C:\\Users\\Sinjun\\Desktop\\Assignment 1 - files\\Task1files\\logo_shuffled.txt";
	input_data_shuffle = readTXT(inputFileName, M, N);

	if (File_has_read)//check the that text files where both read
	{

		BinaryImage noise(M, N, input_data_noise, thresh);//store the noiseed image
		BinaryImage shuffle(M, N, input_data_shuffle, thresh);//store the shuffled image

		BinaryImage E(M, N, 0.0, thresh);//will be used to store the compared blocks

		double SSD = 0;// to store the SSD 

		for (int i = 0; i < 16; i++)
		for (int j = 0; j < 16; j++){
			BinaryImage C = shuffle.getblock(i * 32, i * 32 + 31, j * 32, j * 32 + 31);//get a block from unnoised, to compare agienst

			for (int ii = 0; ii < 16; ii++)
			for (int jj = 0; jj < 16; jj++){//check agienst 16 times
				BinaryImage	D = noise.getblock(ii * 32, ii * 32 + 31, jj * 32, jj * 32 + 31);//take a block from shuffled, to compare agienst

				//loop & nest for finding the SSD
				for (int k = 0; k < 32; k++)
				for (int l = 0; l < 32; l++)
				{
					SSD += (D.get(k, l) - C.get(k, l)) * (D.get(k, l) - C.get(k, l));//work out the diff
				}

				if (SSD == 0)// must of found a match
				{//store the location of the match found into E
					for (int b = 0; b < 32; b++)
					for (int v = 0; v < 32; v++)
					{
						E.set((ii * 32) + b, (jj * 32) + v, (C.get(b, v)));//save location 
					}
				}
				SSD = 0; // reset the SSD
			}
		}
		cout << "-------------- Comparing images complete --------------" << endl << endl;

		for (int i = 0; i < 512; i++)
		for (int j = 0; j < 512; j++)
		{
			input_data_shuffle[count] = E.get(i, j);
			count++;
		}

		// Use Q = 255 for greyscale images and 1 for binary images.
		int Q = 1;

		//outfile save loaction
		char*outputFileName = "C:\\Users\\\Sinjun\\Desktop\\Assignment 1 - files\\Task1files\\logo_unshuffled.pgm";
		WritePGM(outputFileName, input_data_shuffle, M, N, Q);
		
		cout << "Image called \"logo_unshuffled.pgm\" was save to Loaction:" << endl << outputFileName << endl << endl;
	}
	
	system("pause");

	delete[] input_data_noise;
	delete[] input_data_shuffle;

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
			
			i++;
		}
		myfile.close();
	}

	else
	{
		cout << "Unable to open file at loaction:" << endl << fileName << endl << endl;
		File_has_read = false;
	}
	
	return data;
}

// convert data from double to .pgm stored in filename
void WritePGM(char *filename, double *data, int sizeR, int sizeC, int Q)
{

	int i, j;
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
