//BinaryImage function .cpp file
#include "BinaryImage.h"
//BinaryImage constructors
BinaryImage::BinaryImage(int M, int N, double* input_data, double thresh) :Matrix(M, N, input_data)
{
	for (int i = 0; i < M; i++){
		for (int j = 0; j<N; j++){
			if (input_data[i*N + j]>thresh) // remove noise from image
			{
				data[i*N + j] = 1;
			}
			else
				data[i*N + j] = 0;
					}
	}
}
BinaryImage::BinaryImage(int R, int C, double val, double thresh) :Matrix(R, C, val)
{
	data = new double[M*N];

	for (int ii = 0; ii < M; ii++){
		for (int jj = 0; jj < N; jj++){
			data[ii*N + jj] = val;
		}
	}

}
//copy constructor
BinaryImage::BinaryImage(const BinaryImage& X) :Matrix(X)
{
	M = X.M;
	N = X.N;
	data = new double[M*N];

	for (int i = 0; i < M*N; i++)
	{
		data[i] = X.data[i];

	}
}


//BinaryImage operators
BinaryImage BinaryImage::operator+(const BinaryImage& X)
{
	//using Bitwise OR logic, if ethier data == 1 or if both then val = 1
	double val = 0;
	BinaryImage Z(M, N, val, 150);
	for (int i = 0; i < M; i++)
	for (int j = 0; j < N; j++){
		if (data[i*N + j] == 0 && X(i, j) == 0){
			val = 0;
		}
		else
			val = 1;
		Z.set(i, j, val);
	}
	return Z;
}
BinaryImage BinaryImage::operator-(const BinaryImage& X)
{
	//using Bitwise XOR logic, if both data == 1 or 0 then val = 0
	double val = 0;
	int i, j;
	BinaryImage Z(M, N, val, 150);
	for (i = 0; i < M; i++)
	for (j = 0; j < N; j++){

		if (data[i*N + j] == X.data[i*N + j]){
			val = 0;
		}
		else{
			val = 1.0;
		}
		Z.set(i, j, val);
	}
	Z.Output_Matrix(M, N);
	return Z;
}
BinaryImage BinaryImage::operator*(const BinaryImage& X)
{

	//using Bitwise AND logic, only if both set == 1, than val = 1
	double val = 0;
	BinaryImage Z(M, N, val, 150);
	for (int i = 0; i < M; i++)
	for (int j = 0; j < N; j++){
		if (data[i*N + j] == 1 && X(i, j) == 1){
			val = 1;
		}
		else
			val = 0;
		Z.set(i, j, val);
	}
	return Z;
}

//BinaryImage getblock
BinaryImage BinaryImage::getblock(int start_row, int end_row, int start_col, int end_col)
{

	//get a block of data from another BinaryImage to make a new BinaryImage
	BinaryImage X(end_row - start_row + 1, end_col - start_col + 1, 0.0, 170);

	for (int i = start_row; i <= end_row; i++)
	{
		for (int j = start_col; j <= end_col; j++)
		{
			X.set(i - start_row, j - start_col, data[i*N + j]);
		}
	}
	return X;
}