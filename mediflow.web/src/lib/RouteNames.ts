export type RouteNames = typeof RouteNames;

export const RouteNames = {
	//auth
	Login: '/auth/login',
	Join: '/auth/join',
	Register: '/auth/register',

	//admin
	AdminHome: '/admin/home',
	AdminStructure: '/admin/structures',

	//manager
	Manage: '/structure/manage',

	//shared
	Index: '/',
	Home: '/home',
	CurrentStructure: '/structure',
	Profile: (userId: string) => `/profile/${userId}`,
	CurrentProfile: '/profile/my',

	//client
	ClientsIndex: '/clients',
	Client: (clientId: string) => `/clients/${clientId}`,
	EditClient: (clientId: string) => `/clients/${clientId}/edit`,

	//journal
	JournalIndex: '/journal',
	Notes: (clientId: string) => `/journal/${clientId}/notes`,
	NoteOverview: (params: { clientId: string; noteId: string }) =>
		`/journal/${params.clientId}/notes/${params.noteId}`,
	EditNote: (params: { clientId: string; noteId: string }) =>
		`/journal/${params.clientId}/notes/${params.noteId}/edit`,
	WriteNote: (clientId: string) => `/journal/${clientId}/notes/today/edit`,
	TodaysNote: (clientId: string) => `/journal/${clientId}/notes/today`,
};
