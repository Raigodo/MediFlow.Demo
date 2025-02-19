export const baseUrl = 'http://localhost:8080';

export const baseUrlWithPath = (path: string) => {
	if (!path.startsWith('/')) {
		return `${baseUrl}/${path}`;
	}
	return `${baseUrl}${path}`;
};
