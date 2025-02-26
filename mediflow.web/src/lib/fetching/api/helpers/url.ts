export const baseUrl = '';

export const baseUrlWithPath = (path: string) => {
	if (!path.startsWith('/')) {
		return `${baseUrl}/${path}`;
	}
	return `${baseUrl}${path}`;
};
