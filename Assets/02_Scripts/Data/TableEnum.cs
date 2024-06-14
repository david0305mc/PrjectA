public enum QST_MISSION_TYPE 
{ 
	NONE                         = 0,        	// 없음
	PRODUCT_MAKE                 = 1,        	// (생산품) 생산
	PRODUCT_SELL                 = 2,        	// (생산품) 판매
	PRODUCT_PRESENT              = 3,        	// (생산품) 선물하기
	BUILDING_BUILD               = 4,        	// (건물) 건설
	BUILDING_UPGRADE             = 5,        	// (건물) 업그레이드
	BUILDING_VISIT               = 6,        	// (건물) 방문
	GARDEN_BUILD                 = 7,        	// (조경) 건설
	EXTENSION                    = 8,        	// 지역 확장
	GACHA                        = 9,        	// 캐릭터 가차
	MINIGAME                     = 10,       	// (미니게임)플레이
	COLLECT                      = 11,       	// 채집
	MATERIAL_BUY                 = 12,       	// (재료) 구매
	MATERIAL_SELL                = 13,       	// (재료) 판매
	COSTUME_BUY                  = 14,       	// (코스튬) 구매
	FRIEND_ADD                   = 15,       	// 친구 추가
	FRIEND_COUNT                 = 16,       	// 친구 수 달성
	REQUEST_NORMAL               = 17,       	// 일반 의뢰 완료
	REQUEST_SPECIAL              = 18,       	// 특별 의뢰 완료
	VILLAGER_FAVOR               = 19,       	// 빌리저 호감도 달성
	VILLAGER_LEVEL               = 20,       	// 빌리저 레벨 달성
	USER_LEVEL                   = 21,       	// 유저 레벨 달성
	CLEAN                        = 22,       	// 청정도 달성
	FRIEND_TOWN                  = 23,       	// 친구 타운 방문
	VILLAGER_STORY               = 24,       	// 빌리저의 이야기듣기(스토리 보기)
	GUILD_MAKE                   = 25,       	// 길드 생성/가입
	GUILD_CONTENT                = 26,       	// 길드 컨텐츠
	BANK_EXCHANGE                = 27,       	// 환전
	BANK_SAVE                    = 28,       	// 저축 금액
	TRADE                        = 29,       	// 교역
	FISHING                      = 30,       	// 낚시
	HUNTING                      = 31,       	// 사냥
	BARTER                       = 32,       	// 재료/생산품 교환
	MYHOUSE_GIFT                 = 33,       	// (마이하우스) 선물하기
	EQUIP_POWER_UP               = 34,       	// (마이하우스) 소지품 강화하기
	EQUIP_BREAK                  = 35,       	// (마이하우스) 소지품 분해하기
	MYHOUSE_SET_ICON             = 36,       	// (마이하우스) 프로필 아이콘 설정하기
	MYHOUSE_SET_LEADER           = 37,       	// (마이하우스) 대표 주민 설정하기
	MYHOUSE_SET_BG               = 38,       	// (마이하우스) 배경 설정하기
	BUILDING_REMODELING          = 39,       	// (건축사무소) 리모델링 하기
	MAILBOX_SHOW_AD              = 40,       	// (소식함) 광고 보기
	CAFE_INTERVIEW               = 41,       	// (카페) 인터뷰 하기
	CAFE_RADIO                   = 42,       	// (카페) 라디오 방송하기
	CAFE_RADIO_GUEST             = 43,       	// (카페) 라디오 게스트 초대하기
	CAFE_DATE                    = 44,       	// (카페) 식사 초대하기
	COLLECT_GRADE                = 45,       	// (채집) 특정 등급 이상 채집물 획득하기
	GOLD_USED                    = 46,       	// 골드 사용하기
	GOLD_GATHER                  = 47,       	// 골드 획득하기
	VILLAGER_STAR_LEVEL          = 48,       	// 빌리저 별 레벨 달성
}
public enum PASS_TYPE 
{ 
	NONE                         = 0,        	// 패스 불가
	GOLD                         = 1,        	// 골드
	SEED                         = 2,        	// SEED
}
public enum QST_REQ_TYPE 
{ 
	NONE                         = 0,        	// 없음
	USER_LEVEL                   = 1,        	// 유저 레벨
	QUEST                        = 2,        	// 선행 퀘스트(퀘스트 인덱스)
	VILLAGER_FAVOR               = 3,        	// 호감도 레벨
	VILLIGER_GET                 = 4,        	// 빌리저 보유(빌리저 인덱스)
	KEY                          = 5,        	// 스토리 열쇠 소모
}
public enum REWARD_TYPE 
{ 
	NONE                         = 0,        	// 없음
	GOLD                         = 1,        	// 골드
	SEED                         = 2,        	// SEED(유료)
	SEED_FREE                    = 3,        	// SEED(무료)
	EXP                          = 4,        	// 타운경험치(계정)
	EXP_VILLAGER                 = 5,        	// 빌리져경험치
	FAVOR                        = 6,        	// 호감도
	MILEAGE                      = 7,        	// 마일리지
	ITEM                         = 8,        	// 아이템
	VILLAGER                     = 9,        	// 빌리져
	EQUIP                        = 10,       	// 소지품
	BLUEPRINT                    = 11,       	// 설계도 포인트
	STORY                        = 12,       	// 이벤트 스토리
}
public enum STRUCTURE_TYPE 
{ 
	NONE                         = 0,        	// 없음
	FARM                         = 1,        	// 밭
	BARN                         = 2,        	// 축사
	FACTORY                      = 3,        	// 공장
	STORE                        = 4,        	// 전문점
	RESTAURANT                   = 5,        	// 레스토랑
	ROAD                         = 6,        	// 길
	HILL_1                       = 7,        	// 1층 언덕
	HILL_2                       = 8,        	// 2층 언덕
	STAIR_1                      = 9,        	// 1층 계단
	STAIR_2                      = 10,       	// 2층 계단
	STAIR_3                      = 11,       	// 1층>2층 계단
	EXTPANSION                   = 99,       	// 개발 구역
	SYSTEM                       = 100,      	// 시스템건물
}
public enum QST_TYPE 
{ 
	NONE                         = 0,        	// 없음
	MAIN                         = 1,        	// 퀘스트
	VILLAGER                     = 2,        	// 주민 스토리
	RELATION                     = 3,        	// 인연 스토리
	EVENT                        = 4,        	// 이벤트 스토리
	MAIN_STORY                   = 5,        	// 메인 스토리
}
public enum STORY_TYPE 
{ 
	NONE                         = 0,        	// 없음
	MAIN                         = 1,        	// 메인 스토리
	VILLAGER                     = 2,        	// 주민 스토리
	RELATION                     = 3,        	// 인연 스토리
	EVENT                        = 4,        	// 이벤트 스토리
}
public enum GRADE_TYPE 
{ 
	NONE                         = 0,        	// 없음
	C                            = 1,        	// C등급
	B                            = 2,        	// B등급
	A                            = 3,        	// A등급
	S                            = 4,        	// S등급
	SS                           = 5,        	// SS등급
}
public enum EQUIP_SLOT_TYPE 
{ 
	NONE                         = 0,        	// 없음
	RED                          = 1,        	// 빨강(획득량 증가)
	BLUE                         = 2,        	// 파랑(비용 감소)
	PURPLE                       = 3,        	// 보라(시간 감소)
	GREEN                        = 4,        	// 녹색(채집시 특정 등급 획득 확률 증가)
}
public enum SKILL_TYPE 
{ 
	NONE                         = 0,        	// 없음
	FRIEND                       = 1,        	// 프렌드 스킬
	SET                          = 2,        	// 세트 스킬
	ABILITY                      = 3,        	// 능력치 스킬
	EQUIP                        = 4,        	// 소지품
}
public enum SKILL_EFFECT_TYPE 
{ 
	NONE                         = 0,        	// 없음
	QST_GOLD                     = 1,        	// 퀘스트 골드 보상 증가
	QST_EXP                      = 2,        	// 퀘스트 경험치 보상 증가
	REQ_SLOT                     = 3,        	// 일반 의뢰 슬롯 개수 증가
	REQ_GOLD                     = 4,        	// 일반 의뢰 골드 보상 증가
	REQ_EXP                      = 5,        	// 일반 의뢰 경험치 보상 증가
	REQS_GET                     = 6,        	// 특별 의뢰 획득 확률 증가
	REQS_TIMELIMIT               = 7,        	// 특별 의뢰 제한 시간 증가
	REQ_WAIT                     = 8,        	// 일반 의뢰 포기시 대기 시간 감소
	STRUCTURE_GOLD               = 9,        	// 건물 골드 구입 비용 감소
	STRUCTURE_TIME               = 10,       	// 건축 시간 감소
	REMODEL_TIME                 = 11,       	// 리모델링 시간 감소
	REMODEL_GOLD                 = 12,       	// 리모델링 골드 비용 감소
	UPGRADE_TIME                 = 13,       	// 업그레이드 시간 감소
	UPGRADE_GOLD                 = 14,       	// 업그레이드 골드 비용 감소
	MINIGAME_REWARD              = 15,       	// 미니게임 보상 증가
	MTRL_BUY                     = 16,       	// 재료 구매 가격 감소
	MTRL_SELL                    = 17,       	// 재료 판매 가격 증가
	GUILD_POINT                  = 18,       	// 길드 점수 획득량 증가
	ADD_CLEAR_POINT              = 19,       	// 청정도 추가
	INCREASE_MINI_POINT1         = 20,       	// 미니게임1 점수 획득량 증가
	INCREASE_MINI_POINT2         = 21,       	// 미니게임2 점수 획득량 증가
	INCREASE_MINI_POINT3         = 22,       	// 미니게임3 점수 획득량 증가
	INCREASE_MINI_POINT4         = 23,       	// 미니게임4 점수 획득량 증가
	INCREASE_MINI_POINT5         = 24,       	// 미니게임5 점수 획득량 증가
	INCREASE_RENTAL_CNT          = 25,       	// 친구 주민 빌려쓰기 횟수 증가
	ABILITY_GOLD                 = 1001,     	// 생산 비용 감소
	ABILITY_MTRL1_REQ            = 1002,     	// 1차 재료 요구량 감소
	ABILITY_MTRL2_REQ            = 1003,     	// 2차 재료 요구량 감소
	ABILITY_MTRL3_REQ            = 1004,     	// 3차 재료 요구량 감소
	ABILITY_MTRL4_REQ            = 1005,     	// 4차 재료 요구량 감소
	ABILITY_TIME                 = 1006,     	// 생산 시간 감소
	ABILITY_MAKE_RATE            = 1007,     	// 생산량 증가(확률)
	ABILITY_GET                  = 1008,     	// 특정 등급 획득 확률 증가
	ABILITY_EXP_TOWN             = 1009,     	// 타운 경험치 획득량 증가
	ABILITY_EXP_VILLAGER         = 1010,     	// 빌리저 경험치 획득량 증가
	ABILITY_EXP_FAVOR            = 1011,     	// 호감도 획득량 증가
	ABILITY_EXPLORE_RATE_B       = 1012,     	// 특정 채집 지역에서 B등급 채집물 획득 확률 증가
	ABILITY_EXPLORE_RATE_A       = 1013,     	// 특정 채집 지역에서 A등급 채집물 획득 확률 증가
	ABILITY_EXPLORE_RATE_S       = 1014,     	// 특정 채집 지역에서 S등급 채집물 획득 확률 증가
	ABILITY_EXPLORE_RATE_SS      = 1015,     	// 특정 채집 지역에서 SS등급 채집물 획득 확률 증가
}
public enum RATIO_TYPE 
{ 
	NONE                         = 0,        	// 없음
	FIX                          = 1,        	// 스킬 계수를 고정값으로 적용
	RATE                         = 2,        	// 스킬 계수를 천분율로 적용
}
public enum EMOTION_TYPE 
{ 
	NORMAL                       = 0,        	// 기본 표정
	JOY                          = 1,        	// 기쁨
	EXCITING                     = 2,        	// 신남
	SHY                          = 3,        	// 수줍음
	SAD                          = 4,        	// 슬픔
	AWKWARD                      = 5,        	// 어색함
	RESENT                       = 6,        	// 억울함
	SHAME                        = 7,        	// 창피함
	FEAR                         = 8,        	// 무서움
	ANGRY                        = 9,        	// 화남
	JEALOUSY                     = 10,       	// 질투
	TUMULT                       = 11,       	// 혼란스러움
	PRIDE                        = 12,       	// 자랑스러움
}
public enum JOB_TYPE 
{ 
	NONE                         = 0,        	// 노비스
	FARM                         = 1,        	// 농부
	BARN                         = 2,        	// 수의사
	FACTORY                      = 3,        	// 기술자
	SHOP                         = 4,        	// 웨이터
	RESTAURANT                   = 5,        	// 요리사
}
public enum REQUEST_TYPE 
{ 
	NONE                         = 0,        	// 없음
	EASY                         = 1,        	// 쉬움
	NORMAL                       = 2,        	// 보통
	HARD                         = 3,        	// 어려움
	SPECIAL                      = 4,        	// 특별
}
public enum ITEM_TYPE 
{ 
	NONE                         = 0,        	// 없음
	PRODUCT                      = 1,        	// 생산품
	COLLECT                      = 2,        	// 채집물
	LETTER                       = 3,        	// 빌리저 편지(조각)
	EQUIP_UPGRADE                = 4,        	// 소지품 강화 재료
	FAVOR_EXP                    = 5,        	// 호감도 선물
	VILLAGER_EXP                 = 6,        	// 빌리저 경험치 선물
	TIMECUT                      = 7,        	// 시간 단축 아이템
	GACHA                        = 8,        	// 뽑기권
	RANDOMBOX                    = 9,        	// 랜덤박스
	SALECOUPON                   = 10,       	// 할인 쿠폰
	FRIENDSCOIN                  = 11,       	// 우정 포인트
}
public enum PRICE_TYPE 
{ 
	NONE                         = 0,        	// 구매불가
	GOLD                         = 1,        	// 골드
	SEED                         = 2,        	// SEED
	MILEAGE                      = 3,        	// 마일리지
	MONEY                        = 4,        	// 현금
	BLUEPRINT                    = 5,        	// 설계도 포인트
	ADVERTISE                    = 6,        	// 광고
}
public enum ICON_TAG 
{ 
	NONE                         = 0,        	// 없음
	NEW                          = 1,        	// NEW
	SALE                         = 2,        	// SALE
	LIMITED                      = 3,        	// 한정 상품
	EVENT                        = 4,        	// EVENT
}
public enum OPEN_CONDITION 
{ 
	NONE                         = 0,        	// 없음
	USER_LEVEL                   = 1,        	// 타운 레벨
	VILLAGER_GET                 = 2,        	// 특정 주민 보유 (주민 인덱스)
	VILLAGER_STAR                = 3,        	// 특정 주민 별등급
	VILLAGER_FAVOR               = 4,        	// 특정 주민 호감도
	VILLAGER_LEVEL               = 5,        	// 특정 주민 레벨
	USED_GOLD                    = 6,        	// 골드 사용량
	USED_SEED                    = 7,        	// 시드 사용량
	FAVOR_MAX_CNT                = 8,        	// 호감도 Max 주민 수
	ITEM_GET_CNT                 = 9,        	// 특정 아이템 일정 횟수 획득
	REACH_RANKING                = 10,       	// 특정 컨텐츠 일정 랭킹 달성
}
public enum SALE_GROUP 
{ 
	ALWAYS                       = 0,        	// 항시 판매
	BASIC                        = 1,        	// 기본 그룹
	FIRST                        = 2,        	// 최초 그룹
	COUPON_BASIC                 = 3,        	// 할인 쿠폰 적용 기본 그룹
	COUPON_FIRST                 = 4,        	// 할인 쿠폰 적용 최초 그룹
}
public enum SHOP_CATEGORY 
{ 
	NONE                         = 0,        	// 없음
	FARM                         = 1,        	// 밭
	BARN                         = 2,        	// 축사
	FACTORY                      = 3,        	// 공장
	STORE                        = 4,        	// 전문점
	RESTAURANT                   = 5,        	// 레스토랑
	PROP                         = 11,       	// 소품
	ROAD                         = 12,       	// 도로
	WOOD                         = 13,       	// 나무
	WALL                         = 14,       	// 담벼락
	WATER                        = 15,       	// 물
	HILL                         = 16,       	// 언덕
	STRUCTURE                    = 17,       	// 건물
}
public enum MARKET_CATEGORY 
{ 
	NONE                         = 0,        	// 없음
	PACKAGE                      = 1,        	// 패키지
	MONEY                        = 2,        	// 재화
	ITEM                         = 3,        	// 아이템
	MILEAGE                      = 4,        	// 마일리지
	SPECIAL                      = 5,        	// 스페셜
	SPECIAL_PASS                 = 6,        	// 콘테스트 스페셜 패스
}
public enum VILLAGER_UNION_TYPE 
{ 
	NONE                         = 0,        	// 없음
	CLOVER                       = 1,        	// 클로버마을주민
	HURSLEY                      = 2,        	// 허슬리기업
	INTERPOL                     = 3,        	// 인터폴
	CATASTROPH                   = 4,        	// 카타스트로프
	STAR                         = 5,        	// 연예계
}
public enum GACHA_TYPE 
{ 
	NONE                         = 0,        	// 없음
	VILLIGER_GACHA               = 1,        	// 캐릭터가차
	VILLIGER_UNLIMIT_GACHA       = 2,        	// 무제한캐릭터가차
	THING_GACHA                  = 3,        	// 소지품가차
	THING_UNLIMIT_GACHA          = 4,        	// 무제한소지품가차
	VILLIGER_SET_GACHA           = 5,        	// 확정 캐릭터 가차
	VILLAGER_RETRY_GACHA         = 6,        	// 무제한다시뽑기가차
}
public enum CONSTRUCTION_TABS 
{ 
	NONE                         = 0,        	// 없음
	MANUFACTURE                  = 1,        	// 생산시설
	DECO                         = 2,        	// 꾸미기
	REMODELING                   = 3,        	// 리모델링
	BLUEPRINT                    = 4,        	// 설계도
	EXCHANGE                     = 5,        	// 교환소
}
public enum BOOK_TYPE 
{ 
	NONE                         = 0,        	// 없음
	BOOK_VILLAGER                = 1,        	// 주민 도감
	BOOK_PRODUCT                 = 2,        	// 생산/채집 도감
	BOOK_EQUIP                   = 3,        	// 소지품 도감
}
public enum BOOK_CATEGORY 
{ 
	NONE                         = 0,        	// 없음
	CLOVER                       = 11,       	// 클로버 타운
	CATASTROPH                   = 12,       	// 카타스트로프
	INTERPOL                     = 13,       	// 인터폴리스
	HURSLEY                      = 14,       	// 허슬리 코퍼레이션
	ENTERTAINER                  = 15,       	// 연예계
	FARM                         = 21,       	// 밭
	BARN                         = 22,       	// 축사
	FACTORY                      = 23,       	// 공장
	STORE                        = 24,       	// 전문점
	RESTAURANT                   = 25,       	// 레스토랑
	FOREST                       = 31,       	// 숲
	MINE                         = 32,       	// 광산
	CAVE                         = 33,       	// 동굴
	RED                          = 41,       	// 빨간 슬롯
	GREEN                        = 42,       	// 초록 슬롯
	BLUE                         = 43,       	// 파란 슬롯
	PURPLE                       = 44,       	// 보라 슬롯
}
public enum HISTORY_TYPE 
{ 
	NONE                         = 0,        	// 없음
	GATHER_VILLAGER              = 1,        	// 주민 등록
	DONE_MISSION                 = 2,        	// 의뢰 해결
	PRODUCE_CNT_FARM             = 3,        	// 밭 생산 횟수
	PRODUCE_CNT_PEN              = 4,        	// 축사 생산 횟수
	PRODUCE_CNT_FACTORY          = 5,        	// 공장 생산 횟수
	PRODUCE_CNT_STORE            = 6,        	// 전문점 생산 횟수
	PRODUCE_CNT_RESTAURANT       = 7,        	// 레스토랑 생산 횟수
	PRODUCE_CNT_FOREST           = 8,        	// 숲 채집 횟수
	PRODUCE_CNT_MINE             = 9,        	// 광산 채집 횟수
	PRODUCE_CNT_CAVE             = 10,       	// 동굴 채집 횟수
	VISIT_OTHER_VILLAGE          = 11,       	// 다른 마을 방문
	TOTAL_VISITOR                = 12,       	// 총 방문자
	FINISH_CNT_DAILYMISSION      = 13,       	// 일일 미션 달성
	FINISH_CNT_WEEKLYMISSION     = 14,       	// 주간 미션 달성
	USE_BUILD_OFFICE             = 15,       	// 건축사무소 이용
	ACHIEVE_THEME                = 16,       	// 테마 달성
	JOIN_EVENT_CNT               = 17,       	// 이벤트 참여
}
public enum PLAY_STATE_TYPE 
{ 
	NONE                         = 0,        	// 없음
	APPEAR_GACHA                 = 1,        	// 가챠 등장 대사
	ENTER_MYHOUSE_N              = 2,        	// 마이하우스 진입 (일반)
	ENTER_MYHOUSE_D              = 3,        	// 마이하우스 진입 (데레데레)
	ENTER_MYHOUSE_G              = 4,        	// 마이하우스 진입 (글루미)
	INTERATION_VILLAGER_N        = 5,        	// 인터렉션
	INTERATION_VILLAGER_D        = 6,        	// 인터렉션 (데레데레)
	INTERATION_VILLAGER_G        = 7,        	// 인터렉션 (글루미)
	INTERATION_VILLAGER_S        = 8,        	// 인터렉션 (스페셜
	GET_NORMAL_FOOD              = 9,        	// 일반적인 음식 구성
	GET_LIKE_FOOD                = 10,       	// 좋아하는 음식이 포함된 구성
	GET_DISLIKE_FOOD             = 11,       	// 싫어하는 음식이 포함된 구성
}
public enum FOOD_GROUP 
{ 
	NONE                         = 0,        	// 없음
	FOOD_MAIN                    = 1,        	// 음식
	FOOD_DESSERT                 = 2,        	// 디저트
	FOOD_DRINK                   = 3,        	// 음료
}
public enum CONTENT_GROUP 
{ 
	NONE                         = 0,        	// 없음
	CLEAN                        = 1,        	// 청정도
	PRODUCT_POWER                = 2,        	// 생산력
	MISSION_COMPLETE             = 3,        	// 의뢰 해결
	EXPLORE_RATE                 = 4,        	// 탐험도(채집)
	MINIGAME_SCORE_1             = 5,        	// 미니게임1 스코어
	POPULARITY_RATE              = 6,        	// 인기도
	SUPPORT_PT                   = 7,        	// 응원 포인트
}
public enum EVENT_TYPE 
{ 
	ROULETTE                     = 1,        	// 룰렛
	BINGO                        = 2,        	// 빙고
	LOGINBONUS                   = 3,        	// 이벤트출석
}
public enum QUEST_TYPE 
{ 
	NONE                         = 0,        	// 없음
	DAILY                        = 1,        	// 일일 미션(매일 출석 갱신 시간에 리셋
	WEEKLY                       = 2,        	// 주간 미션(매주 월요일 출석 갱신 시간에 리셋
	EVENT                        = 3,        	// 이벤트 미션(이벤트가 종료되면 리셋
	REPEAT                       = 4,        	// 반복 미션(이벤트가 종료되면 리셋
}
public enum RESET_TYPE 
{ 
	NONE                         = 0,        	// 없음
	RESET_DAILY                  = 1,        	// 일간 추기화
	RESET_WEEKLY                 = 2,        	// 주간 초기화
	RESET_MONTHLY                = 3,        	// 월간 초기화
	PASS                         = 4,        	// 월정액 패스
}
public enum SCENARIO_ACT_TYPE 
{ 
	NONE                         = 0,        	// 없음
	APPEAR                       = 1,        	// 등장
	LEAVE                        = 2,        	// 퇴장
	TALK                         = 3,        	// 대화 출력
	CHOOSE                       = 4,        	// 선택지 출력
	CHAPTER                      = 5,        	// 챕터
	PLACE                        = 6,        	// 장소 창 출력
	NARRATION                    = 7,        	// 나레이션 연출
	CHANGE_BG                    = 8,        	// 배경 교체
	BGM_START                    = 9,        	// BGM 재생 시작
	BGM_END                      = 10,       	// BGM 재생 종료
	WHITE_FLASH                  = 11,       	// 화면 반짝임(점멸) 효과 연출
	BLACK_FADE                   = 12,       	// 검은색 페이드 인/아웃 연출
	ANI_CHANGE                   = 13,       	// 애니메이션만 교체
	END_EFFECT                   = 14,       	// 시나리오 종료 연출
}
public enum GUIDE_TYPE 
{ 
	NONE                         = 0,        	// 없음
	TOWN_GUIDE                   = 1,        	// 타운 가이드
	CONTENT_GUIDE                = 2,        	// 컨텐츠 가이드
}
public enum GUIDE_UI_TYPE 
{ 
	NONE                         = 0,        	// 없음
	OBJECT_TYPE                  = 1,        	// Object형
	UI_TYPE                      = 2,        	// UI형
}
public enum MAIL_TYPE 
{ 
	NONE                         = 0,        	// 없음
	RENT                         = 1,        	// 빌려쓰기 보상 우편
	ADMIN                        = 2,        	// 운영 우편
	ADVERTISE                    = 3,        	// 광고 우편
	COUPON                       = 4,        	// 쿠폰 우편
}
public enum MAIL_CATEGORY 
{ 
	NONE                         = 0,        	// 없음
	NORMAL                       = 1,        	// 일반
	ADMIN                        = 2,        	// 운영
	LOG                          = 3,        	// 로그
}
public enum SUPPORT_LANGUAGE 
{ 
	EN                           = 1,        	// 영어
	KR                           = 2,        	// 한국어
	JP                           = 3,        	// 일본어
	GAN                          = 4,        	// 간체
	BUN                          = 5,        	// 번체
}
public enum SYSTEM_TYPE 
{ 
	REQUEST                      = 1,        	// 의뢰
	GACHA                        = 2,        	// 기차역
	COLLECT                      = 3,        	// 등산로
	FRIEND                       = 4,        	// 친구
	NEWSPAPER                    = 5,        	// 신문사
}
public enum AD_TYPE 
{ 
	MAILBOX_REWARD               = 1,        	// 소식함 보상 광고
	PROMISE_REFRESH              = 2,        	// 내일의 약속 무료 갱신 광고
	FRIENDS_GACHA                = 3,        	// 우정 가챠 1회 무료 광고
	SHOP_SEED                    = 4,        	// 시장 무료 시드 상품 광고
}
public enum INTERACT_TYPE 
{ 
	NONE                         = 0,        	// 없음
	ACTION                       = 1,        	// 개별 인터렉션
	SUMMON                       = 2,        	// 소집용 오브젝트
}
public enum PUSH_TYPE 
{ 
	NONE                         = 0,        	// 없음
	FIELD_PUSH                   = 1,        	// 밭 생산 완료
	BARN_PUSH                    = 2,        	// 축사 생산 완료
	FACTORY_PUSH                 = 3,        	// 공장 생산 완료
	STORE_PUSH                   = 4,        	// 전문점 생산 완료
	RESTAURANT_PUSH              = 5,        	// 레스토랑 생산 완료
	STRUCTURE_PUSH               = 6,        	// 건설 완료
	UPGRADE_PUSH                 = 7,        	// 업그레이드 완료
	EXPLORE_PUSH                 = 8,        	// 채집 완료
	MISSYOU_PUSH                 = 9,        	// 장기 미접속 (7일 미접속 시 보냄
	REQUEST_FRIEND_PUSH          = 10,       	// 친구 신청 알림
	NOTICE_POSTBOX_PUSH          = 11,       	// 신규 소식함 수신
}
