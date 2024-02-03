class_name World extends Node

@onready var tileMap: TileMap = $StoneTileMap

enum TileType {
	STONE,
	COAL,
	NIL
}

var rng = RandomNumberGenerator.new()

class ChunkGenerator:
	var pos: Vector2
	var dir: Vector2

var CellSize = Vector2(16, 16)

@export var WIDTH: int = 256
@export var HEIGHT: int = 256

var MAP_WIDTH = WIDTH/CellSize.x
var MAP_HEIGHT = HEIGHT/CellSize.y
@export var SAFE_ZONE_RADIUS = 5

var ORE_CHUNK_CHANCE = 0.1
var GENERATOR_MOVE_CHANCE = 0.5
var GENERATOR_DESTROY_CHANCE = 0.8
var GENERATOR_SPAWN_CHANCE = 0.1

var MAX_ITERATION = 10
var MAX_GENERATORS = 5
var MAX_FIL_PERCENT = 0.3

var DIRECTIONS: Array[Vector2] = [Vector2(-1, 0), Vector2(1, 0), Vector2(0, 1), Vector2(0, -1)]

var grid = []

var generators: Array[ChunkGenerator] = []


func getRandomDirection() -> Vector2:
	return DIRECTIONS[rng.randi()%4]
	
func isInSafeZone(x: int, y: int) -> bool:
	var t_x = x - MAP_WIDTH/2
	var t_y = y - MAP_HEIGHT/2
	var radius = sqrt(pow(t_x, 2) + pow(t_y,2))
	return radius < SAFE_ZONE_RADIUS
	

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
	generator.pos = Vector2(MAP_WIDTH/2, MAP_HEIGHT/2)
	generators = [generator]

func createChunks():
	initGenerators()
	var iteration: int = 0
	var tile_count: int = 0
	while iteration < MAX_ITERATION:
		# Change direction, with chance
		for i in range(generators.size()):
			if rng.randf() < GENERATOR_MOVE_CHANCE:
				generators[i].dir = getRandomDirection()
		
		# Random: Maybe destroy walker?
		for i in range(generators.size()):
			if rng.randf() < GENERATOR_DESTROY_CHANCE and generators.size() > 1:
				generators.remove_at(i);
				break; # Destroy only one walker per iteration
		
		# Spawn new walkers, with chance
		for i in range(generators.size()):
			if rng.randf() < GENERATOR_SPAWN_CHANCE and generators.size() < MAX_GENERATORS:
				var generator = ChunkGenerator.new()
				generator.dir = getRandomDirection()
				generator.pos = generators[i].pos
				generators.append(generator)
	
		# Advance walkers
		for i in range(generators.size()):
			var generator = generators[i]
			
			if (generator.pos.x + generator.dir.x >= 1 and 
				generator.pos.x + generator.dir.x < MAP_WIDTH-1 and
				generator.pos.y + generator.dir.y >= 1 and
				generator.pos.y + generator.dir.y < MAP_HEIGHT-1):
					
					generator.pos += generator.dir
					if grid[generator.pos.x][generator.pos.y] == TileType.STONE:
						grid[generator.pos.x][generator.pos.y] = TileType.COAL
						tile_count += 1
						
						if float(tile_count)/float(MAP_WIDTH*MAP_HEIGHT) >= MAX_FIL_PERCENT:
							return
		
		iteration += 1
		print("Tile count: ",tile_count)


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
					tileMap.set_cell(0, Vector2(t_x, t_y), 0, Vector2(2, 2))
				TileType.COAL:
					print("Setting coal", x, " ", y)
					tileMap.set_cell(1, Vector2(t_x, t_y), 0, Vector2(0, 1))
					

# Called when the node enters the scene tree for the first time.
func _ready():
	initGrid()
	initGenerators()
	createChunks()
	# printChunks()
	spawnTiles()
	


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
