export type HttpResponse<Tbody> = {
	ok: boolean;
	status: number;
	message: string;
	errors: Record<string, string[]> | null;
	body: Tbody | null;
};

export type PendingHttpResponse<Tbody> = Promise<HttpResponse<Tbody>>;

export type MakeHttpRequestOptionsType = {
	method?: 'GET' | 'POST' | 'PATCH' | 'PUT' | 'DELETE';
	body?: any;
	headers?: HeadersInit;
	requestContentType?: 'JSON' | 'BLOB';
	responseContentType?: 'JSON' | 'BLOB';
	credentials?: RequestCredentials;
};

export const makeHttpRequest = async <Tbody>(
	customFetch = fetch,
	url: string,
	opt: MakeHttpRequestOptionsType = {},
): PendingHttpResponse<Tbody> => {
	opt.method ??= 'GET';
	opt.body ??= undefined;
	opt.headers ??= {};
	opt.requestContentType ??= 'JSON';
	opt.responseContentType ??= 'JSON';
	opt.credentials ??= 'include';

	if (opt.method.toUpperCase() === 'GET' && opt.body)
		throw Error('are you drunk? who puts body to GET requests...');

	try {
		const fetchOptions =
			opt.requestContentType === 'JSON'
				? {
						method: opt.method,
						headers: {
							'Content-type': 'application/json',
							...opt.headers,
						},
						body: opt.body && JSON.stringify(opt.body),
						credentials: opt.credentials,
				  }
				: {
						method: opt.method,
						headers: opt.headers,
						body: opt.body,
						credentials: opt.credentials,
				  };
		const res = await customFetch(url, fetchOptions);
		switch (true) {
			case res.status >= 500: {
				return {
					ok: false,
					status: res.status,
					message: 'server failure',
					errors: null,
					body: null,
				};
			}

			case res.status === 401: {
				return {
					ok: false,
					status: res.status,
					message:
						'can not complete authorized request without valid session',
					errors: null,
					body: null,
				};
			}

			case res.status === 403: {
				return {
					ok: false,
					status: res.status,
					message: 'session has no rights to execute action',
					errors: null,
					body: null,
				};
			}

			case res.status >= 400: {
				const json: {
					message: string;
					errors: Record<string, string[]>;
				} = await res.json().catch(() => ({
					message: 'server does not provide any payload in response',
					errors: {},
				}));
				return {
					ok: false,
					status: res.status,
					message: json.message,
					errors: json.errors ?? {},
					body: null,
				};
			}

			case res.status >= 300: {
				return {
					ok: false,
					status: res.status,
					message: 'redirecting',
					errors: null,
					body: null,
				};
			}

			case res.status >= 200: {
				const body: Tbody | null =
					opt.responseContentType === 'JSON'
						? await res.json().catch(() => null)
						: await res.blob().catch(() => null);
				return {
					ok: true,
					status: res.status,
					message: 'success',
					errors: null,
					body,
				};
			}

			case res.status < 200: {
				return {
					ok: false,
					status: res.status,
					message: 'strangely got response with 1xx status code',
					errors: null,
					body: null,
				};
			}

			default: {
				return {
					ok: false,
					status: res.status,
					message: `something was not expected in makeHttpRequest and default case was hit`,
					errors: null,
					body: null,
				};
			}
		}
	} catch (e) {
		console.error(e);
		return {
			ok: false,
			status: 0,
			message: 'something went wrong',
			errors: null,
			body: null,
		};
	}
};
