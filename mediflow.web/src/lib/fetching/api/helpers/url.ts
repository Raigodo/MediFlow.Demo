export const baseUrl = process.env.NEXT_PUBLIC_API_URL;
console.log(process.env.TEST_VAR);

export const baseUrlWithPath = (path: string) => {
	if (!path.startsWith('/')) {
		return `${baseUrl}/${path}`;
	}
	return `${baseUrl}${path}`;
};
