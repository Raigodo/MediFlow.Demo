export type StructureEntity = {
	id: string;
	name: string;
	manager: {
		userId: string;
		name: string;
		surname: string;
	};
};
