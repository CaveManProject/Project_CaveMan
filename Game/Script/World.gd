class_name World extends Node

@onready var tileMap: TileMap = $StoneTileMap
@onready var player: PlayerBody = $Player

enum TileType {
	STONE,
	COAL,
	NIL
}

func typeToCords(type: TileType) -> Vector2:
	match type:
		TileType.STONE: return Vector2(2, 2)
		TileType.COAL: return Vector2(0, 1)
	return Vector2(-1, -1)

func cordsToType(cords: Vector2) -> TileType: 
	if cords.x == 2 and cords.y == 2:
		return TileType.STONE
	if cords.x == 0 and cords.y == 1:
		return TileType.COAL
	return TileType.NIL

var rng = RandomNumberGenerator.new()

@export var MAX_CHUNK_SIZE: int = 8

class ChunkGenerator:
	var pos: Vector2
	var dir: Vector2
	var chunk_size: int

var CellSize = Vector2(16, 16)

@export var MAP_WIDTH: int = 20
@export var MAP_HEIGHT: int = 20

@export var SAFE_ZONE_RADIUS: int = 4

@export var GENERATOR_DESTROY_CHANCE: float = 0.1
@export var GENERATOR_SPAWN_CHANCE: float = 1

@export var MAX_ITERATION: int = 10
@export var MAX_GENERATORS: int = 10

var DIRECTIONS: Array[Vector2] = [Vector2(-1, 0), Vector2(1, 0), Vector2(0, 1), Vector2(0, -1)]

var grid = []

var generators: Array[ChunkGenerator] = []

func isInSafeZone(x: int, y: int) -> bool:
	var t_x = x - MAP_WIDTH/2
	var t_y = y - MAP_HEIGHT/2
	var radius = sqrt(pow(t_x, 2) + pow(t_y,2))
	return radius < SAFE_ZONE_RADIUS

func getRandomDirection() -> Vector2:
	return DIRECTIONS[rng.randi()%4]

func getSafeRandomPosition() -> Vector2:
	var ran_x = rng.randi_range(1, MAP_WIDTH-1)
	var ran_y = rng.randi_range(1, MAP_HEIGHT-1)
	while isInSafeZone(ran_x, ran_y):
		ran_x = rng.randi_range(1, MAP_WIDTH-1)
		ran_y = rng.randi_range(1, MAP_HEIGHT-1)
	return Vector2(ran_x, ran_y)

func initGrid():
	grid = []
	for x in range(MAP_WIDTH):
		grid.append([])
		for y in range(MAP_HEIGHT):
			if isInSafeZone(x,y):
				grid[x].append(TileType.NIL)
			else:
				grid[x].append(TileType.STONE)

func initGenerators():
	var generator: ChunkGenerator = ChunkGenerator.new()
	generator.dir = getRandomDirection()
	generator.pos = getSafeRandomPosition()
	generator.chunk_size = 0
	generators = [generator]

func createChunks():
	initGenerators()
	var iteration: int = 0
	while iteration < MAX_ITERATION:
		# Change direction, with chance
		for generator in generators:
			generator.dir = getRandomDirection()
			
		# Random: Maybe destroy generator?
		for generator in generators:
			if rng.randf() < GENERATOR_DESTROY_CHANCE:
				generators.erase(generator);
				break; # Destroy only one generator per iteration
		
		# Spawn new generator, with chance
		for _g in generators:
			if rng.randf() < GENERATOR_SPAWN_CHANCE and generators.size() < MAX_GENERATORS:
				var generator = ChunkGenerator.new()
				generator.dir = getRandomDirection()
				generator.pos = getSafeRandomPosition()
				print("Spawning at: ", generator.pos)
				generator.chunk_size = 0
				generators.append(generator)
	
		# Advance generator
		for generator in generators:
			if (generator.pos.x + generator.dir.x >= 1 and 
				generator.pos.x + generator.dir.x < MAP_WIDTH-1 and
				generator.pos.y + generator.dir.y >= 1 and
				generator.pos.y + generator.dir.y < MAP_HEIGHT-1):
					
					generator.pos += generator.dir
					if grid[generator.pos.x][generator.pos.y] == TileType.STONE:
						grid[generator.pos.x][generator.pos.y] = TileType.COAL
						generator.chunk_size += 1
						
						if generator.chunk_size == MAX_CHUNK_SIZE:
							generators.erase(generator)
		
		iteration += 1


func printChunks():
	for x in range(MAP_WIDTH):
		for y in range(MAP_HEIGHT):
			if grid[x][y] != TileType.STONE:
				print(grid[x][y])

func spawnTiles():
	for x in MAP_WIDTH:
		for y in MAP_HEIGHT:
			var t_x = x - MAP_WIDTH/2
			var t_y = y - MAP_HEIGHT/2
			match grid[x][y]:
				TileType.STONE:
					tileMap.set_cell(0, Vector2(t_x, t_y), 0, typeToCords(TileType.STONE))
				TileType.COAL:
					tileMap.set_cell(0, Vector2(t_x, t_y), 0, typeToCords(TileType.COAL))
					

# Called when the node enters the scene tree for the first time.
func _ready():
	initGrid()
	initGenerators()
	createChunks()
	spawnTiles()

func blockInRadius(playerCords: Vector2) -> bool:
	for dir in DIRECTIONS:
		var newDir = playerCords + dir
		if grid[newDir.x][newDir.y] != TileType.NIL:
			return true
	return false

func mineBlock(playerCords: Vector2):
	for dir in DIRECTIONS:
		var newDir = playerCords + dir
		if grid[newDir.x][newDir.y] != TileType.NIL:
			tileMap.erase_cell(0, newDir)

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	var playerCoords = tileMap.local_to_map(player.global_position)
	if Input.is_action_just_pressed("e") and blockInRadius(playerCoords) :
		mineBlock(playerCoords)
	
	if Input.is_action_just_pressed("space"):
		print("Generating")
		initGrid()
		initGenerators()
		createChunks()
		spawnTiles()
		
		
