function  V = deriv(U)

% DERIV Column-wise derivative estimation.
%	Computes 5-point discrete derivative estimates for each column
%	of the input matrix U.
%	V = DERIV(U)   Returns a matrix V of the same size as U each
%	element containing the estimates of the column-wise derivative of
%	a function sampled at unit intervals in U.
%
%	DERIV Uses 5 points both in the interior and at the edges.
%	If size of the matrix U is less than 5 use simpler 3-point
%	or 2-point estimate.
%
%	See also DIFF, GRADIENT, DEL2

%  Kirill K. Pankratov,  kirill@plume.mit.edu
%  March 22, 1994

 % 5-point coefficients for derivatives ..................................
 % Edges (1st, 2nd pts):
Cde = [-25/12 4 -3 4/3 -1/4; -1/4 -5/6 3/2 -1/2 1/12];
 % Interior
Cd = [1/12 -2/3 0 2/3 -1/12];

 % Determine the size of the input and make column if vector .............
ist = 0;
ly = size(U,1);
if ly==1, ist = 1; U = U(:); ly = length(U); end

 % If only 2 points - simple difference ..................................
if ly==2
  V(1,:) = U(2,:)-U(1,:);
  V(2,:) = V(1,:);
  if ist, V = V'; end     % Transpose output if necessary
  return
end

 % Now if more than 2 points - more complicated procedure ...............
if ly<5         % If less than 5 points

  V(2:ly-1,:) = (U(3:ly,:)-U(1:ly-2,:))/2;        % First
  V(1,:) = 2*U(2,:)-1.5*U(1,:)-.5*U(3,:);         % Last
  V(ly,:) = 1.5*U(ly,:)-2*U(ly-1,:)+.5*U(ly-2,:); % Interior

else            % If 5 points or more - regular, more accurate estimation

   % Edges ............
  V(1:2,:) = Cde*U(1:5,:);
  V([ly ly-1],:) = -fliplr(Cde)*U(ly-4:ly,:);

   % Interior .........
  V(3:ly-2,:) = Cd(1)*U(1:ly-4,:)+Cd(2)*U(2:ly-3,:)+Cd(3)*U(3:ly-2,:);
  V(3:ly-2,:) = V(3:ly-2,:)+Cd(4)*U(4:ly-1,:)+Cd(5)*U(5:ly,:);

end

if ist, V = V'; end     % Transpose output if necessary
