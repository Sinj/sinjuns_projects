function findspuds2( raw_image )
pop_img = imread(raw_image);
%
% read in  6 background
bk1 = uint16 (imread('emptybelt1.jpg'));
bk2 = uint16 (imread('emptybelt2.jpg'));
bk3 = uint16 (imread('emptybelt3.jpg'));
bk4 = uint16 (imread('emptybelt4.jpg'));
bk5 = uint16 (imread('emptybelt5.jpg'));
bk6 = uint16 (imread('emptybelt6.jpg'));
%imshow(mat2gray(bk4));

[x, y, d] = size(pop_img); % get dimensions
avg = [x,y,d];% set up dimensions 
%  do loop to add all the depth togather
for i = 1:d
    for j = 1:y
        for k = 1:x
            avg(k, j, i) = (bk1(k, j, i) + bk2(k, j, i) + bk3(k, j, i) + bk4(k, j, i) + bk5(k, j, i) + bk6(k, j, i))/6;
        end
    end
end

%figure, imshow(mat2gray(avg));



% take raw_image, covernt 2 bw, invert image(find threshoeld of bw image)
% convert 2 bw
bw_img = im2bw(pop_img, 0.16);
%figure, imshow(bw_img);

bw2_img = imcomplement(bw_img);
figure, imshow(pop_img);
figure, imshow(bw2_img);
% do image moringfing (slide 6)

% go thro the invert bw image, and replace pixals that are white to the equivilante mean belt image pixals

for i = 1:d
    for j = 1:y %cols
        for k = 1:x %rows
            if (bw2_img(k, j) > 0)
                pop_img(k, j, i) = avg(k, j, i);
            end
        end
    end
end
figure, imshow(pop_img);

%do colour slice

% erode - to clean image of noise

% take stuff from task1 to do boxing and other segmetiaion and stat
% gathering

end

