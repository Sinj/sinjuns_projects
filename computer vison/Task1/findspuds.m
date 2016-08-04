function findspuds( raw_img )
figure, imshow(raw_img);
pop_img = imread(raw_img);

% --------------------------------------Pre-processing start---------------
% convert 2 grey - so can use filter
grey_img = rgb2gray(pop_img);
% figure, imshow(grey_img);

%filter - to remove noise
grey_img = medfilt2(grey_img);
% figure, imshow(grey_img);

% convert 2 bw - part of filling in holes
for i = 1:324
    for j = 1:576
        if(grey_img(i, j) > 19)
            bw_img(i, j) = 1;
        else
            bw_img(i, j) = 0;
        end
    end
end

% invert image
bw2_img = imcomplement(bw_img);


% inclearboarder - remove any thin touching the boarder
icb_img = imclearborder(bw2_img,4);


% add bw image & clear board image - filled on holes
new_img = icb_img + bw_img;


% erogde image - makes the image smaller so potatols dnt touch
se = strel('disk',5);
img_erod = imerode(new_img,se);

% --------------------------------------Find objects-----------------------

% bwlable to count
[L, num] = bwlabel(img_erod);
lab_img = label2rgb(L, 'jet', 'g', 'shuffle');
% figure, imshow(lab_img);

%display the number of potatos
disp(['The numbers of potatos found: ' num2str(num)]);

% regionprops
img_stat = regionprops(L,'all');

Potato_array = [];% used to store object data

% -----------------------------Segmentation and boxing start---------------

for i=1:numel(img_stat)
    if img_stat(i).Area>80
        % do boarder around each image
        rectangle('Position',img_stat(i).BoundingBox, 'LineWidth',2,...
            'EdgeColor','g', 'LineStyle', '-');
        
        %crop the oringinal image to get stat
        croped_Img = imcrop(pop_img, img_stat(i).BoundingBox);
        gray_Croped_Img = imcrop(grey_img, img_stat(i).BoundingBox);
        
        % mean & STD for RBG.
        for j=1:3
            %loop img
            crop_image = mean(croped_Img(:, :, j));
            HoldVal = 0;
            
            % add the colounms of mean
            for k=1:numel(crop_image)
                HoldVal = crop_image(k) + HoldVal;
            end
            % word out mean
            HoldVal = HoldVal/numel(crop_image);
            
            % using the loop for each channle to get the STD and
            % avaegre/mean
            if j == 1
                MeanRed = HoldVal;
                StdDevRed = std2(croped_Img(:, :, j));
            elseif j == 2
                MeanGreen = HoldVal;
                StdDevGreen = std2(croped_Img(:, :, j));
            else
                MeanBlue = HoldVal;
                StdDevBlue = std2(croped_Img(:, :, j));
            end
        end
        
        %crop the grey image to get entropy
        gray_Croped_Img = imcrop(grey_img, img_stat(i).BoundingBox);
        
        %entropy
        entpy = entropy(gray_Croped_Img);
        
        % work out the smoothness
        StdDevMean = (StdDevBlue+StdDevGreen+StdDevRed)/3;
        smooth = 1-(1/(1+StdDevMean));
        %fill arry
        if ~(i > 1)
            Potato_array = [i, img_stat(i).Centroid(1), img_stat(i).Centroid(2),...
                img_stat(i).Eccentricity, MeanRed, MeanGreen, MeanBlue, StdDevRed,...
                StdDevGreen StdDevBlue, smooth, entpy;];
        else % added more to the arry
            Potato_array = [Potato_array; i, img_stat(i).Centroid(1), img_stat(i).Centroid(2),...
                img_stat(i).Eccentricity, MeanRed, MeanGreen, MeanBlue, StdDevRed,...
                StdDevGreen, StdDevBlue, smooth, entpy;];
        end
    end
end
% outputs table
Potato_table = array2table(Potato_array,...
    'VariableNames',{'Potato','Centroid_X','Centroid_Y','Eccentricity','Mean_Red','Mean_Green' 'Mean_Blue','std_Dev_Red','std_Dev_Green','std_Dev_Blue','Smoothness','Entropy'});

disp(Potato_table);

end

